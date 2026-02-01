using PersonalSite.Domain.Interfaces.Repositories.Analytics;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;
using PersonalSite.Domain.Interfaces.Repositories.User;
using PersonalSite.Infrastructure.Persistence.Repositories.Analytics;
using PersonalSite.Infrastructure.Persistence.Repositories.Blog;
using PersonalSite.Infrastructure.Persistence.Repositories.Common;
using PersonalSite.Infrastructure.Persistence.Repositories.Contact;
using PersonalSite.Infrastructure.Persistence.Repositories.Pages;
using PersonalSite.Infrastructure.Persistence.Repositories.Projects;
using PersonalSite.Infrastructure.Persistence.Repositories.Skills;
using PersonalSite.Infrastructure.Persistence.Repositories.Translations;
using PersonalSite.Infrastructure.Persistence.Repositories.User;

namespace PersonalSite.Infrastructure.DependencyInjection;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAnalyticsEventRepository, AnalyticsEventRepository>();
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        services.AddScoped<IBlogPostTagRepository, BlogPostTagRepository>();
        services.AddScoped<IPostTagRepository, PostTagRepository>();
        services.AddScoped<ISocialMediaLinkRepository, SocialMediaLinkRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();
        services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ILearningSkillRepository, LearningSkillRepository>();
        services.AddScoped<IProjectSkillRepository, ProjectSkillRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<ISkillCategoryRepository, SkillCategoryRepository>();
        services.AddScoped<IUserSkillRepository, UserSkillRepository>();
        services.AddScoped<IBlogPostTranslationRepository, BlogPostTranslationRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IPageTranslationRepository, PageTranslationRepository>();
        services.AddScoped<IProjectTranslationRepository, ProjectTranslationRepository>();
        services.AddScoped<ISkillCategoryTranslationRepository, SkillCategoryTranslationRepository>();
        services.AddScoped<ISkillTranslationRepository, SkillTranslationRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}