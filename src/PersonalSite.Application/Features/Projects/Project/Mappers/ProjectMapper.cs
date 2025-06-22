namespace PersonalSite.Application.Features.Projects.Project.Mappers;

public class ProjectMapper 
    : ITranslatableMapper<Domain.Entities.Projects.Project, ProjectDto>, 
    IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto>
{
    private readonly IS3UrlBuilder _s3UrlBuilder;
    private readonly ITranslatableMapper<ProjectSkill, ProjectSkillDto> _skillMapper;
    private readonly IAdminMapper<ProjectSkill, ProjectSkillAdminDto> _skillAdminMapper;
    private readonly IMapper<ProjectTranslation, ProjectTranslationDto> _translationMapper;

    public ProjectMapper(
        IS3UrlBuilder s3UrlBuilder,
        ITranslatableMapper<ProjectSkill, ProjectSkillDto> skillMapper,
        IAdminMapper<ProjectSkill, ProjectSkillAdminDto> skillAdminMapper,
        IMapper<ProjectTranslation, ProjectTranslationDto> translationMapper)
    {
        _s3UrlBuilder = s3UrlBuilder;   
        _skillMapper = skillMapper;   
        _skillAdminMapper = skillAdminMapper; 
        _translationMapper = translationMapper;  
    }
    
    public ProjectDto MapToDto(Domain.Entities.Projects.Project entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));

        return new ProjectDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = _s3UrlBuilder.BuildUrl(entity.CoverImage),
            DemoUrl = entity.DemoUrl,
            RepoUrl = entity.RepoUrl,
            Title = translation?.Title ?? string.Empty,
            ShortDescription = translation?.ShortDescription ?? string.Empty,
            DescriptionSections = translation?.DescriptionSections ?? new Dictionary<string, string>(),
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) 
                ? string.Empty 
                : _s3UrlBuilder.BuildUrl(translation.OgImage),
            Skills = _skillMapper.MapToDtoList(entity.ProjectSkills, languageCode)
        };
    }
    
    public List<ProjectDto> MapToDtoList(IEnumerable<Domain.Entities.Projects.Project> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public ProjectAdminDto MapToAdminDto(Domain.Entities.Projects.Project entity)
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
            Translations = _translationMapper.MapToDtoList(entity.Translations),
            Skills = _skillAdminMapper.MapToAdminDtoList(entity.ProjectSkills)
        };
    }
    
    public List<ProjectAdminDto> MapToAdminDtoList(IEnumerable<Domain.Entities.Projects.Project> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}