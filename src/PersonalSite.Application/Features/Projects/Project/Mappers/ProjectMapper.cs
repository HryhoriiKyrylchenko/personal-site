namespace PersonalSite.Application.Features.Projects.Project.Mappers;

public static class ProjectMapper
{
    public static ProjectDto MapToDto(Domain.Entities.Projects.Project entity, string languageCode)
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
            Skills = SkillMapper.MapToDtoList(entity.ProjectSkills.Select(ps => ps.Skill), languageCode)
        };
    }
    
    public static List<ProjectDto> MapToDtoList(IEnumerable<Domain.Entities.Projects.Project> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public static ProjectAdminDto MapToAdminDto(Domain.Entities.Projects.Project entity)
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
            Translations = ProjectTranslationMapper.MapToDtoList(entity.Translations),
            Skills = SkillMapper.MapToAdminDtoList(entity.ProjectSkills.Select(ps => ps.Skill))
        };
    }
    
    public static List<ProjectAdminDto> MapToAdminDtoList(IEnumerable<Domain.Entities.Projects.Project> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}