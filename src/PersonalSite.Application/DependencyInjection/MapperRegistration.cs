using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Mappers;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Mappers;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Language.Mappers;
using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Application.Features.Common.LogEntries.Mappers;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.Resume.Mappers;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Mappers;
using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Application.Features.Contact.ContactMessages.Mappers;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Mappers;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Projects.Project.Mappers;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.LearningSkills.Mappers;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Mappers;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Mappers;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Mappers;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.DependencyInjection;

public static class MapperRegistration
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly? assembly = null)
    {
        // services.AddScoped<IMapper<Domain.Entities.Analytics.AnalyticsEvent, AnalyticsEventDto>, AnalyticsEventMapper>();
        // services.AddScoped<ITranslatableMapper<BlogPost, BlogPostDto>, BlogPostMapper>();
        // services.AddScoped<IAdminMapper<BlogPost, BlogPostAdminDto>, BlogPostMapper>();
        // services.AddScoped<IMapper<BlogPostTag, BlogPostTagDto>, BlogPostTagMapper>();
        // services.AddScoped<IMapper<BlogPostTranslation, BlogPostTranslationDto>, BlogPostTranslationMapper>();
        // services.AddScoped<IMapper<Language, LanguageDto>, LanguageMapper>();
        // services.AddScoped<IMapper<LogEntry, LogEntryDto>, LogEntryMapper>();
        // services.AddScoped<IMapper<Resume, ResumeDto>, ResumeMapper>();
        // services.AddScoped<IMapper<SocialMediaLink, SocialMediaLinkDto>, SocialMediaLinkMapper>();
        // services.AddScoped<IMapper<ContactMessage, ContactMessageDto>, ContactMessageMapper>();
        // services.AddScoped<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>, PageMapper>();
        // services.AddScoped<IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto>, PageMapper>();
        // services.AddScoped<IMapper<PageTranslation, PageTranslationDto>, PageTranslationMapper>();
        // services.AddScoped<ITranslatableMapper<Domain.Entities.Projects.Project, ProjectDto>, ProjectMapper>();
        // services.AddScoped<IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto>, ProjectMapper>();
        // services.AddScoped<ITranslatableMapper<ProjectSkill, ProjectSkillDto>, ProjectSkillMapper>();
        // services.AddScoped<IAdminMapper<ProjectSkill, ProjectSkillAdminDto>, ProjectSkillMapper>();
        // services.AddScoped<IMapper<ProjectTranslation, ProjectTranslationDto>, ProjectTranslationMapper>();
        // services.AddScoped<ITranslatableMapper<LearningSkill, LearningSkillDto>, LearningSkillMapper>();
        // services.AddScoped<IAdminMapper<LearningSkill, LearningSkillAdminDto>, LearningSkillMapper>();
        // services.AddScoped<ITranslatableMapper<SkillCategory, SkillCategoryDto>, SkillCategoryMapper>();
        // services.AddScoped<IAdminMapper<SkillCategory, SkillCategoryAdminDto>, SkillCategoryMapper>();
        // services.AddScoped<IMapper<SkillCategoryTranslation, SkillCategoryTranslationDto>, SkillCategoryTranslationMapper>();
        // services.AddScoped<ITranslatableMapper<Skill, SkillDto>, SkillMapper>();
        // services.AddScoped<IAdminMapper<Skill, SkillAdminDto>, SkillMapper>();
        // services.AddScoped<IMapper<SkillTranslation, SkillTranslationDto>, SkillTranslationMapper>();
        // services.AddScoped<ITranslatableMapper<UserSkill, UserSkillDto>, UserSkillMapper>();
        // services.AddScoped<IAdminMapper<UserSkill, UserSkillAdminDto>, UserSkillMapper>();
        // return services;
        
        assembly ??= Assembly.GetExecutingAssembly();

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(IMapper<,>),
                typeof(ITranslatableMapper<,>),
                typeof(IAdminMapper<,>)
            ))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        return services;
    }
}