using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Features.Projects.Project.Mappers;

public class ProjectTranslationMapper : IMapper<ProjectTranslation, ProjectTranslationDto>
{
    private readonly IS3UrlBuilder _s3UrlBuilder;
    
    public ProjectTranslationMapper(IS3UrlBuilder s3UrlBuilder)
    {
        _s3UrlBuilder = s3UrlBuilder;   
    }
    
    public ProjectTranslationDto MapToDto(ProjectTranslation entity)
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
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : _s3UrlBuilder.BuildUrl(entity.OgImage)
        };
    }
    
    public List<ProjectTranslationDto> MapToDtoList(
        IEnumerable<ProjectTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}