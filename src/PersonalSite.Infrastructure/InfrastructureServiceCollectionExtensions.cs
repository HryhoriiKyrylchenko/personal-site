namespace PersonalSite.Infrastructure;

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
        
        services.AddScoped<IStorageService, S3StorageService>();
        
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        
        services.AddScoped<IAnalyticsEventRepository, AnalyticsEventRepository>();
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        services.AddScoped<IBlogPostTagRepository, BlogPostTagRepository>();
        services.AddScoped<IPostTagRepository, PostTagRepository>();
        services.AddScoped<ILogEntryRepository, LogEntryRepository>();
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
        
        return services;
    }
}