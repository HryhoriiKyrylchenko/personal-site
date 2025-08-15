using Npgsql;
using PersonalSite.Application.DependencyInjection;
using PersonalSite.Infrastructure.DependencyInjection;
using PersonalSite.Web.Configuration;
using PersonalSite.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

SerilogConfigurator.Configure(builder.Configuration);

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
    
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

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.Configure<AwsS3Settings>(builder.Configuration.GetSection("AwsS3Settings"));
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddScoped<IStorageService, S3StorageService>();
builder.Services.AddSingleton<IS3UrlBuilder, S3UrlBuilder>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontend");

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