using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.ApplicationCore.Services.Interfaces;
using GtMotive.Estimate.Microservice.Domain.MongoDb.Settings;
using GtMotive.Estimate.Microservice.Domain.Repositories;
using GtMotive.Estimate.Microservice.Domain.Repositories.Interfaces;
using GtMotive.Estimate.Microservice.Host.Configuration;
using GtMotive.Estimate.Microservice.Host.DependencyInjection;
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

// Configuration of logging for host startup.
builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

// MongoDB configuration (only in development).
if (builder.Environment.IsDevelopment())
{
    var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()
        ?? throw new InvalidOperationException("MongoDB configuration is not defined in appsettings.Development.json.");

    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

    // Register IMongoDatabase in the dependency injection container.
    builder.Services.AddSingleton(mongoDatabase);
}
else
{
    // Azure Key Vault configuration in production.
    builder.Configuration.AddJsonFile("serilogsettings.json", optional: false, reloadOnChange: true);

    var secretClient = new SecretClient(
        new Uri($"https://{builder.Configuration.GetValue<string>("KeyVaultName")}.vault.azure.net/"),
        new DefaultAzureCredential());

    builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
}

// Add services to the dependency injection container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AppSettings configuration.
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

// MongoDB configuration in production (if necessary).
if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
}

// Register custom services.
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleController, VehicleController>();

// Authentication configuration.
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

// Swagger configuration.
builder.Services.AddSwagger(appSettings, builder.Configuration);

// ForwardedHeaders configuration.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Logging configuration.
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

// PathBase configuration.
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

// Swagger configuration.
app.UseSwaggerInApplication(pathBase, builder.Configuration);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
