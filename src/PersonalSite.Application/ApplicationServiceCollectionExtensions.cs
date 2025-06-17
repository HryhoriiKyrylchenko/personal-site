using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;

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
        
        return services;
    }
}