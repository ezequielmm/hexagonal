using System;
using System.Globalization;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// Configuración del logging para el arranque del host.
builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

// Añadir servicios al contenedor de dependencias.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de MongoDB.
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()
    ?? throw new InvalidOperationException("La configuración de MongoDB no está definida en appsettings.json.");

var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

// Registrar IMongoDatabase en el contenedor de dependencias.
builder.Services.AddSingleton(mongoDatabase);

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleController, VehicleController>();

var app = builder.Build();

// Configuración del logging.
Log.Logger = builder.Environment.IsDevelopment() ?
    new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            theme: AnsiConsoleTheme.Literate,
            formatProvider: CultureInfo.InvariantCulture)
        .CreateLogger() :
    new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            formatProvider: CultureInfo.InvariantCulture)
        .CreateLogger();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Renting API V1");
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
