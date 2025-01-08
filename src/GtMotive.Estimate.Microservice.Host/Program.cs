using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Host.Configuration;
using GtMotive.Estimate.Microservice.Host.DependencyInjection;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
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

// Configuración de MongoDB (solo en desarrollo).
if (builder.Environment.IsDevelopment())
{
    var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()
        ?? throw new InvalidOperationException("La configuración de MongoDB no está definida en appsettings.Development.json.");

    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

    // Registrar IMongoDatabase en el contenedor de dependencias.
    builder.Services.AddSingleton(mongoDatabase);
}
else
{
    // Configuración de Azure Key Vault en producción.
    builder.Configuration.AddJsonFile("serilogsettings.json", optional: false, reloadOnChange: true);

    var secretClient = new SecretClient(
        new Uri($"https://{builder.Configuration.GetValue<string>("KeyVaultName")}.vault.azure.net/"),
        new DefaultAzureCredential());

    builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
}

// Añadir servicios al contenedor de dependencias.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de AppSettings.
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

// Configuración de MongoDB en producción (si es necesario).
if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
}

// Registrar servicios personalizados.
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleController, VehicleController>();

// Configuración de autenticación.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
})
.AddIdentityServerAuthentication(options =>
{
    options.Authority = appSettings.JwtAuthority;
    options.ApiName = "estimate-api";
    options.SupportedTokens = SupportedTokens.Jwt;
});

// Configuración de Swagger.
builder.Services.AddSwagger(appSettings, builder.Configuration);

// Configuración de ForwardedHeaders.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

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
        .Enrich.WithProperty("Application", "addoperation")
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            formatProvider: CultureInfo.InvariantCulture)
        .WriteTo.ApplicationInsights(
            app.Services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

// Configuración de PathBase.
var pathBase = new PathBase(builder.Configuration.GetValue("PathBase", defaultValue: PathBase.DefaultPathBase));

if (!pathBase.IsDefault)
{
    app.UsePathBase(pathBase.CurrentWithoutTrailingSlash);
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configuración de Swagger.
app.UseSwaggerInApplication(pathBase, builder.Configuration);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
