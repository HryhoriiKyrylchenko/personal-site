namespace PersonalSite.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped(typeof(IReadOnlyRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        
        builder.Services.AddScoped<IAnalyticsEventRepository, AnalyticsEventRepository>();
        builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        builder.Services.AddScoped<IBlogPostTagRepository, BlogPostTagRepository>();
        builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();
        builder.Services.AddScoped<ILogEntryRepository, LogEntryRepository>();
        builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<ILearningSkillRepository, LearningSkillRepository>();
        builder.Services.AddScoped<IProjectSkillRepository, ProjectSkillRepository>();
        builder.Services.AddScoped<ISkillRepository, SkillRepository>();
        builder.Services.AddScoped<ISkillCategoryRepository, SkillCategoryRepository>();
        builder.Services.AddScoped<IUserSkillRepository, UserSkillRepository>();
        builder.Services.AddScoped<IBlogPostTranslationRepository, BlogPostTranslationRepository>();
        builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
        builder.Services.AddScoped<IPageTranslationRepository, PageTranslationRepository>();
        builder.Services.AddScoped<IProjectTranslationRepository, ProjectTranslationRepository>();
        builder.Services.AddScoped<ISkillCategoryTranslationRepository, SkillCategoryTranslationRepository>();
        builder.Services.AddScoped<ISkillTranslationRepository, SkillTranslationRepository>();
        
        return builder;
    }
}