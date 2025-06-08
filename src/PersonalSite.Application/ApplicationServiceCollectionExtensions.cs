namespace PersonalSite.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<LanguageContext>();
        
        services.AddScoped<IAnalyticsEventService, AnalyticsEventService>();
        services.AddScoped<IBlogPostService, BlogPostService>();
        services.AddScoped<IBlogPostTagService, BlogPostTagService>();
        services.AddScoped<ILogEntryService, LogEntryService>();
        services.AddScoped<ISocialMediaLinkService, SocialMediaLinkService>();
        services.AddScoped<IResumeService, ResumeService>();
        services.AddScoped<IContactMessageService, ContactMessageService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ILearningSkillService, LearningSkillService>();
        services.AddScoped<IPageService, PageService>();
        services.AddScoped<IProjectSkillService, ProjectSkillService>();
        services.AddScoped<ISkillCategoryService, SkillCategoryService>();
        services.AddScoped<IUserSkillService, UserSkillService>();
        services.AddScoped<IBlogPostTranslationService, BlogPostTranslationService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IPageTranslationService, PageTranslationService>();
        services.AddScoped<IProjectTranslationService, ProjectTranslationService>();
        services.AddScoped<ISkillCategoryTranslationService, SkillCategoryTranslationService>();
        services.AddScoped<ISkillTranslationService, SkillTranslationService>();
        
        services.AddScoped<IPagesDataService, PagesDataService>();
        services.AddScoped<ISiteInfoService, SiteInfoService>();
        
        return services;
    }
}