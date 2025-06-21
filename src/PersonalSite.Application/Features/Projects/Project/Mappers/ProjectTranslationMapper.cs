namespace PersonalSite.Application.Features.Projects.Project.Mappers;

public static class ProjectTranslationMapper
{
    public static ProjectTranslationDto MapToDto(ProjectTranslation entity)
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
    
    public static List<ProjectTranslationDto> MapToDtoList(
        IEnumerable<ProjectTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}