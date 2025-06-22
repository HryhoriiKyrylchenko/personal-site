namespace PersonalSite.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ContactMessageValidator).Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        
        services.AddSingleton<IBackgroundQueue, BackgroundQueue>();
        services.AddHostedService<QueuedHostedService>();
        
        services.AddHostedService<S3OrphanFileCleanupJob>();
        
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        
        services.AddRepositories();
        
        return services;
    }
}