using PersonalSite.Application.Features.Blogs.Blog.Mappers;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;

namespace PersonalSite.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        
        services.AddScoped<LanguageContext>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetHomePageQuery).Assembly));
        services.AddValidatorsFromAssembly(typeof(SendContactMessageCommandValidator).Assembly);
        
        services.AddMappers(typeof(BlogPostMapper).Assembly);
        
        services.AddScoped<IBackgroundPublisher, BackgroundPublisher>();
        
        services.AddScoped<ILanguageService, LanguageService>();
        
        return services;
    }
}