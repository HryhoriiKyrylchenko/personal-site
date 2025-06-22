namespace PersonalSite.Application.Features.Skills.Skills.Mappers;

public class SkillMapper : ITranslatableMapper<Skill, SkillDto>, IAdminMapper<Skill, SkillAdminDto>
{
    private readonly ITranslatableMapper<SkillCategory, SkillCategoryDto> _categoryMapper;
    private readonly IAdminMapper<SkillCategory, SkillCategoryAdminDto> _categoryAdminMapper;

    public SkillMapper(
        ITranslatableMapper<SkillCategory, SkillCategoryDto> categoryMapper,
        IAdminMapper<SkillCategory, SkillCategoryAdminDto> categoryAdminMapper)
    {
        _categoryMapper = categoryMapper;   
        _categoryAdminMapper = categoryAdminMapper;
    }
    
    public SkillDto MapToDto(Skill entity, string languageCode)
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
            Category = _categoryMapper.MapToDto(entity.Category, languageCode)
        };
    }
    
    public List<SkillDto> MapToDtoList(IEnumerable<Skill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public SkillAdminDto MapToAdminDto(Skill entity)
    {
        return new SkillAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Translations = SkillTranslationMapper.MapToDtoList(entity.Translations),
            Category = _categoryAdminMapper.MapToAdminDto(entity.Category)
        };
    }

    public List<SkillAdminDto> MapToAdminDtoList(IEnumerable<Skill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();   
    }
}