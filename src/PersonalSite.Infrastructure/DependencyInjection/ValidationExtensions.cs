namespace PersonalSite.Infrastructure.DependencyInjection;

public static class ValidationExtensions
{
    public static IServiceCollection AddDomainValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AnalyticsEventValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostTagValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostValidator>();
        services.AddValidatorsFromAssemblyContaining<PostTagValidator>();
        services.AddValidatorsFromAssemblyContaining<LogEntryValidator>();
        services.AddValidatorsFromAssemblyContaining<ResumeValidator>();
        services.AddValidatorsFromAssemblyContaining<SocialMediaLinkValidator>();
        services.AddValidatorsFromAssemblyContaining<ContactMessageValidator>();
        services.AddValidatorsFromAssemblyContaining<PageValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectValidator>();
        services.AddValidatorsFromAssemblyContaining<LearningSkillValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectSkillValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillCategoryValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillValidator>();
        services.AddValidatorsFromAssemblyContaining<UserSkillValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostTranslationValidator>();
        services.AddValidatorsFromAssemblyContaining<LanguageValidator>();
        services.AddValidatorsFromAssemblyContaining<PageTranslationValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectTranslationValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillCategoryTranslationValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillTranslationValidator>();
        
        return services;
    }
}