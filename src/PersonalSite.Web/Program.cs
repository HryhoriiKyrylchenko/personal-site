using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using AspNetCoreRateLimit;
using Npgsql;
using PersonalSite.Application.DependencyInjection;
using PersonalSite.Infrastructure.DependencyInjection;
using PersonalSite.Web.Configuration;
using PersonalSite.Web.Middlewares;

// -------------------------
// Build & Configuration
// -------------------------
var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

if (env.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// -------------------------
// Load AWS secret into configuration (only in Production)
// -------------------------
if (builder.Environment.IsProduction())
{
    const string secretName = "personalsite/production";
    const string region = "eu-north-1";

    var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
    
    var cacheConfig = new SecretCacheConfiguration
    {
        CacheItemTTL = 60 * 60 * 1000u,
        MaxCacheSize = 1024,
        VersionStage = "AWSCURRENT"
    };
    
    var secretCache = new SecretsManagerCache(client, cacheConfig);
    builder.Services.AddSingleton(secretCache);
    
    string secretString;
    try
    {
        secretString = secretCache.GetSecretString(secretName).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading secret '{secretName}': {ex.Message}");
        throw;
    }

    if (!string.IsNullOrEmpty(secretString))
    {
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(secretString));
        builder.Configuration.AddJsonStream(ms);
    }
}

// -------------------------
// AWS SDK integration for other AWS services (S3)
// -------------------------
builder.Services.Configure<AwsS3Settings>(builder.Configuration.GetSection("AwsS3Settings"));
builder.Services.AddAWSService<IAmazonS3>();

// -------------------------
// Serilog
// -------------------------
SerilogConfigurator.Configure(builder.Configuration);
builder.Host.UseSerilog();

// -------------------------
// CORS
// -------------------------
var allowedOriginsPublic = builder.Configuration.GetSection("AllowedOrigins:Public").Get<string[]>() ?? [];
var allowedOriginsAdmin = builder.Configuration["AllowedOrigins:Admin"]?.Split(",") ?? [];

var combinedOrigins = allowedOriginsPublic.Concat(allowedOriginsAdmin).Distinct().ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
        policy.WithOrigins(combinedOrigins)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// -------------------------
// Rate limiting
// -------------------------
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.GeneralRules =
    [
        new RateLimitRule
        {
            Endpoint = "*:/api/analytics/track",
            Period = "1m",
            Limit = 60
        }
    ];
});

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// -------------------------
// Database
// ------------------------    
builder.Services.AddSingleton<NpgsqlDataSource>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.EnableDynamicJson();
    return dataSourceBuilder.Build();
});

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var dataSource = sp.GetRequiredService<NpgsqlDataSource>();
    options.UseNpgsql(dataSource);
});

// -------------------------
// App services
// -------------------------
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.AddScoped<IStorageService, S3StorageService>();
builder.Services.AddSingleton<IS3UrlBuilder, S3UrlBuilder>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseIpRateLimiting();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("DefaultCors");

app.UseMiddleware<AnalyticsMiddleware>();
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

app.Run();