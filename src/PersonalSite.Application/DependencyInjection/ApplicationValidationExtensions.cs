namespace PersonalSite.Application.DependencyInjection;

public static class ApplicationValidationExtensions
{
    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AnalyticsEventAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AnalyticsEventUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostTagAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostTagUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LogEntryAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LogEntryUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ResumeAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ResumeUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SocialMediaLinkAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SocialMediaLinkUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ContactMessageAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ContactMessageUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PageAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PageUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LearningSkillAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LearningSkillUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectSkillAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectSkillUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillCategoryAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillCategoryUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UserSkillAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UserSkillUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostTranslationAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BlogPostTranslationUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LanguageAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LanguageUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PageTranslationAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PageTranslationUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectTranslationAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProjectTranslationUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillCategoryTranslationAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillCategoryTranslationUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillTranslationAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SkillTranslationUpdateRequestValidator>();
        
        return services;
    }
}