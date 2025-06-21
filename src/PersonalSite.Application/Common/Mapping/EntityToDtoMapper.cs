using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;

namespace PersonalSite.Application.Common.Mapping;

public static class EntityToDtoMapper
{
    // TODO: Refactor this class into smaller ones
    
    public static BlogPostDto MapBlogPostToDto(BlogPost entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

        return new BlogPostDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = S3UrlHelper.BuildImageUrl(entity.CoverImage),
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
                
            Title = translation?.Title ?? string.Empty,
            Excerpt = translation?.Excerpt ?? string.Empty,
            Content = translation?.Content ?? string.Empty,
                
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(translation.OgImage),
            
            Tags = MapBlogPostTagsToDtoList(entity.PostTags.Select(pt => pt.BlogPostTag))
        };
    }

    public static List<BlogPostDto> MapBlogPostsToDtoList(IEnumerable<BlogPost> entities, string languageCode)
    {
        return entities.Select(e => MapBlogPostToDto(e, languageCode)).ToList();
    }

    public static BlogPostAdminDto MapBlogPostToAdminDto(BlogPost entity)
    {
        return new BlogPostAdminDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = entity.CoverImage,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsDeleted = entity.IsDeleted,
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
            Translations = MapBlogPostTranslationsToDtoList(entity.Translations),
            Tags = MapBlogPostTagsToDtoList(entity.PostTags.Select(pt => pt.BlogPostTag))
        };
    }

    public static List<BlogPostAdminDto> MapBlogPostsToAdminDtoList(IEnumerable<BlogPost> entities)
    {
        return entities.Select(MapBlogPostToAdminDto).ToList();
    }

    public static ProjectAdminDto MapProjectToAdminDto(Project entity)
    {
        return new ProjectAdminDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = entity.CoverImage,
            DemoUrl = entity.DemoUrl,
            RepoUrl = entity.RepoUrl,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Translations = MapProjectTranslationsToDtoList(entity.Translations),
            Skills = MapSkillsToAdminDtoList(entity.ProjectSkills.Select(ps => ps.Skill))
        };
    }
    
    public static List<ProjectAdminDto> MapProjectsToAdminDtoList(IEnumerable<Project> entities)
    {
        return entities.Select(MapProjectToAdminDto).ToList();
    }

    public static ContactMessageDto MapContactMessageToDto(ContactMessage entity)
    {
        return new ContactMessageDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Subject = entity.Subject,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            IsRead = entity.IsRead
        };
    }
    
    public static List<ContactMessageDto> MapContactMessagesToDtoList(IEnumerable<ContactMessage> entities)
    {
        return entities.Select(MapContactMessageToDto).ToList();
    }

    public static AnalyticsEventDto MapAnalyticsEventToDto(AnalyticsEvent entity)
    {
        return new AnalyticsEventDto
        {
            Id = entity.Id,
            EventType = entity.EventType,
            PageSlug = entity.PageSlug,
            Referrer = entity.Referrer,
            UserAgent = entity.UserAgent,
            CreatedAt = entity.CreatedAt,
            AdditionalDataJson = entity.AdditionalDataJson
        };
    }
    
    public static List<AnalyticsEventDto> MapAnalyticsEventsToDtoList(IEnumerable<AnalyticsEvent> entities)
    {
        return entities.Select(MapAnalyticsEventToDto).ToList();
    }

    public static BlogPostTagDto MapBlogPostTagToDto(BlogPostTag entity)
    {
        return new BlogPostTagDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
    public static List<BlogPostTagDto> MapBlogPostTagsToDtoList(IEnumerable<BlogPostTag> entities)
    {
        return entities.Select(MapBlogPostTagToDto).ToList();
    }

    public static LogEntryDto MapLogEntryToDto(LogEntry entity)
    {
        return new LogEntryDto
        {
            Id = entity.Id,
            Timestamp = entity.Timestamp,
            Level = entity.Level,
            Message = entity.Message,
            MessageTemplate = entity.MessageTemplate,
            Exception = entity.Exception,
            Properties = entity.Properties,
            SourceContext = entity.SourceContext
        };
    }
    
    public static List<LogEntryDto> MapLogEntriesToDtoList(IEnumerable<LogEntry> entities)
    {
        return entities.Select(MapLogEntryToDto).ToList();
    }

    public static SkillCategoryDto MapSkillCategoryToDto(SkillCategory entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));
        
        return new SkillCategoryDto
        {
            Id = entity.Id,
            Key = entity.Key,
            DisplayOrder = entity.DisplayOrder,
            Name = translation?.Name ?? string.Empty,
            Description = translation?.Description ?? string.Empty
        };
    }

    public static List<SkillCategoryDto> MapSkillCategoriesToDtoList(IEnumerable<SkillCategory> entities,
        string languageCode)
    {
        return entities.Select(e => MapSkillCategoryToDto(e, languageCode)).ToList();
    }

    public static SkillDto MapSkillToDto(Skill entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));

        return new SkillDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Name = translation?.Name ?? string.Empty,
            Description = translation?.Description ?? string.Empty,
            Category = MapSkillCategoryToDto(entity.Category, languageCode)
        };
    }
    
    public static List<SkillDto> MapSkillsToDtoList(IEnumerable<Skill> entities, string languageCode)
    {
        return entities.Select(e => MapSkillToDto(e, languageCode)).ToList();
    }

    public static SkillCategoryAdminDto MapSkillCategoryToAdminDto(SkillCategory entity)
    {
        return new SkillCategoryAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            DisplayOrder = entity.DisplayOrder,
            Translations = MapSkillCategoryTranslationsToDtoList(entity.Translations)
        };
    }

    public static List<SkillCategoryAdminDto> MapSkillCategoriesToAdminDtoList(IEnumerable<SkillCategory> entities)
    {
        return entities.Select(MapSkillCategoryToAdminDto).ToList();
    }

    public static SkillAdminDto MapSkillToAdminDto(Skill entity)
    {
        return new SkillAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Translations = MapSkillTranslationsToDtoList(entity.Translations),
            Category = MapSkillCategoryToAdminDto(entity.Category)
        };
    }

    public static List<SkillAdminDto> MapSkillsToAdminDtoList(IEnumerable<Skill> entities)
    {
        return entities.Select(MapSkillToAdminDto).ToList();   
    }
    
    public static ProjectDto MapProjectToDto(Project entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));

        return new ProjectDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = S3UrlHelper.BuildImageUrl(entity.CoverImage),
            DemoUrl = entity.DemoUrl,
            RepoUrl = entity.RepoUrl,
            Title = translation?.Title ?? string.Empty,
            ShortDescription = translation?.ShortDescription ?? string.Empty,
            DescriptionSections = translation?.DescriptionSections ?? new Dictionary<string, string>(),
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(translation.OgImage),
            Skills = MapSkillsToDtoList(entity.ProjectSkills.Select(ps => ps.Skill), languageCode)
        };
    }
    
    public static List<ProjectDto> MapProjectsToDtoList(IEnumerable<Project> entities, string languageCode)
    {
        return entities.Select(e => MapProjectToDto(e, languageCode)).ToList();
    }

    public static LearningSkillAdminDto MapLearningSkillToAdminDto(LearningSkill entity)
    {
        return new LearningSkillAdminDto
        {
            Id = entity.Id,
            Skill = MapSkillToAdminDto(entity.Skill),
            LearningStatus = entity.LearningStatus,
            DisplayOrder = entity.DisplayOrder
        };
    }

    public static List<LearningSkillAdminDto> MapLearningSkillsToAdminDtoList(IEnumerable<LearningSkill> entities)
    {
        return entities.Select(MapLearningSkillToAdminDto).ToList();
    }
    
    public static UserSkillAdminDto MapUserSkillToAdminDto(UserSkill entity)
    {
        return new UserSkillAdminDto
        {
            Id = entity.Id,
            Skill = MapSkillToAdminDto(entity.Skill),
            Proficiency = entity.Proficiency,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static List<UserSkillAdminDto> MapUserSkillsToAdminDtoList(IEnumerable<UserSkill> entities)
    {
        return entities.Select(MapUserSkillToAdminDto).ToList();
    }

    public static UserSkillDto MapUserSkillToDto(UserSkill entity, string languageCode)
    {
        return new UserSkillDto
        {
            Id = entity.Id,
            Skill = MapSkillToDto(entity.Skill, languageCode),
            Proficiency = entity.Proficiency,
        };
    }
    
    public static List<UserSkillDto> MapUserSkillsToDtoList(IEnumerable<UserSkill> entities, string languageCode)
    {
        return entities.Select(e => MapUserSkillToDto(e, languageCode)).ToList();
    }

    public static LearningSkillDto MapLearningSkillToDto(LearningSkill entity, string languageCode)
    {
        return new LearningSkillDto
        {
            Id = entity.Id,
            Skill = MapSkillToDto(entity.Skill, languageCode),
            LearningStatus = entity.LearningStatus.ToString(),
            DisplayOrder = entity.DisplayOrder
        };
    }
    
    public static List<LearningSkillDto> MapLearningSkillsToDtoList(IEnumerable<LearningSkill> entities, string languageCode)
    {
        return entities.Select(e => MapLearningSkillToDto(e, languageCode)).ToList();
    }

    public static PageAdminDto MapPageAdminToDto(Page entity)
    {
        return new PageAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Translations = MapPageTranslationsToDtoList(entity.Translations)
        };
    }

    public static List<PageAdminDto> MapPageAdminsToDtoList(IEnumerable<Page> entities)
    {
        return entities.Select(MapPageAdminToDto).ToList();   
    }

    public static PageTranslationDto MapPageTranslationToDto(PageTranslation entity)
    {
        return new PageTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            PageId = entity.PageId,
            Data = entity.Data,
            Title = entity.Title,
            Description = entity.Description,
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(entity.OgImage)
        };
    }
    
    public static List<PageTranslationDto> MapPageTranslationsToDtoList(IEnumerable<PageTranslation> entities)
    {
        return entities.Select(MapPageTranslationToDto).ToList();
    }

    public static BlogPostTranslationDto MapBlogPostTranslationToDto(BlogPostTranslation entity)
    {
        return new BlogPostTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            BlogPostId = entity.BlogPostId,
            Title = entity.Title,
            Excerpt = entity.Excerpt,
            Content = entity.Content,
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(entity.OgImage)
        };
    }

    public static List<BlogPostTranslationDto> MapBlogPostTranslationsToDtoList(
        IEnumerable<BlogPostTranslation> entities)
    {
        return entities.Select(MapBlogPostTranslationToDto).ToList();
    }
    
    public static LanguageDto MapLanguageToDto(Language entity)
    {
        return new LanguageDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }

    public static List<LanguageDto> MapLanguagesToDtoList(IEnumerable<Language> entities)
    {
        return entities.Select(MapLanguageToDto).ToList();
    }

    public static ProjectTranslationDto MapProjectTranslationToDto(ProjectTranslation entity)
    {
        return new ProjectTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            ProjectId = entity.ProjectId,
            Title = entity.Title,
            ShortDescription = entity.ShortDescription,
            DescriptionSections = entity.DescriptionSections,
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(entity.OgImage)
        };
    }
    
    public static List<ProjectTranslationDto> MapProjectTranslationsToDtoList(
        IEnumerable<ProjectTranslation> entities)
    {
        return entities.Select(MapProjectTranslationToDto).ToList();
    }
    
    public static SkillCategoryTranslationDto MapSkillCategoryTranslationToDto(SkillCategoryTranslation entity)
    {
        return new SkillCategoryTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            SkillCategoryId = entity.SkillCategoryId,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public static List<SkillCategoryTranslationDto> MapSkillCategoryTranslationsToDtoList(
        IEnumerable<SkillCategoryTranslation> entities)
    {
        return entities.Select(MapSkillCategoryTranslationToDto).ToList();
    }

    public static SkillTranslationDto MapSkillTranslationToDto(SkillTranslation entity)
    {
        return new SkillTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            SkillId = entity.SkillId,
            Name = entity.Name,
            Description = entity.Description
        };
    }
    
    public static List<SkillTranslationDto> MapSkillTranslationsToDtoList(
        IEnumerable<SkillTranslation> entities)
    {
        return entities.Select(MapSkillTranslationToDto).ToList();
    }

    public static ProjectSkillDto MapProjectSkillToDto(ProjectSkill entity, string languageCode)
    {
        return new ProjectSkillDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Skill = MapSkillToDto(entity.Skill, languageCode)
        };
    }
    
    public static List<ProjectSkillDto> MapProjectSkillsToDtoList(IEnumerable<ProjectSkill> entities, string languageCode)
    {
        return entities.Select(e => MapProjectSkillToDto(e, languageCode)).ToList();
    }

    public static PageDto MapPageToDto(Page entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(p => p.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));

        return new PageDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Data = translation?.Data ?? new Dictionary<string, string>(),
            Title = translation?.Title ?? string.Empty,
            Description = translation?.Description ?? string.Empty,
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(translation.OgImage)
        };
    }

    public static List<PageDto> MapPagesToDtoList(IEnumerable<Page> entities, string languageCode)
    {
        return entities.Select(e => MapPageToDto(e, languageCode)).ToList();
    }

    public static SocialMediaLinkDto MapSocialMediaLinkToDto(SocialMediaLink entity)
    {
        return new SocialMediaLinkDto
        {
            Id = entity.Id,
            Platform = entity.Platform,
            Url = S3UrlHelper.BuildImageUrl(entity.Url),
            DisplayOrder = entity.DisplayOrder,
            IsActive = entity.IsActive
        };
    }

    public static List<SocialMediaLinkDto> MapSocialMediaLinksToDtoList(IEnumerable<SocialMediaLink> entities)
    {
        return entities.Select(MapSocialMediaLinkToDto).ToList();
    }

    public static ResumeDto MapResumeToDto(Resume entity)
    {
        return new ResumeDto
        {
            Id = entity.Id,
            FileUrl = S3UrlHelper.BuildImageUrl(entity.FileUrl),
            FileName = entity.FileName,
            UploadedAt = entity.UploadedAt,
            IsActive = entity.IsActive
        };
    }
    
    public static List<ResumeDto> MapResumesToDtoList(IEnumerable<Resume> entities)
    {
        return entities.Select(MapResumeToDto).ToList();
    }
}