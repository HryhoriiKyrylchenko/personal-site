namespace PersonalSite.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        
        services.AddScoped<LanguageContext>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetHomePageQuery).Assembly));
        services.AddValidatorsFromAssembly(typeof(SendContactMessageCommandValidator).Assembly);
        
        services.AddScoped<IBackgroundPublisher, BackgroundPublisher>();
        
        services.AddScoped<ILanguageService, LanguageService>();
        
        return services;
    }
}