using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Mappers;

public class SkillCategoryMapper 
    : ITranslatableMapper<SkillCategory, SkillCategoryDto>,
        IAdminMapper<SkillCategory, SkillCategoryAdminDto>
{
    private readonly IMapper<SkillCategoryTranslation, SkillCategoryTranslationDto> _translationMapper;
    
    public SkillCategoryMapper(IMapper<SkillCategoryTranslation, SkillCategoryTranslationDto> translationMapper)
    {
        _translationMapper = translationMapper;
    }
    
    public SkillCategoryDto MapToDto(SkillCategory entity, string languageCode)
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

    public List<SkillCategoryDto> MapToDtoList(IEnumerable<SkillCategory> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public SkillCategoryAdminDto MapToAdminDto(SkillCategory entity)
    {
        return new SkillCategoryAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            DisplayOrder = entity.DisplayOrder,
            Translations = _translationMapper.MapToDtoList(entity.Translations)
        };
    }

    public List<SkillCategoryAdminDto> MapToAdminDtoList(IEnumerable<SkillCategory> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}