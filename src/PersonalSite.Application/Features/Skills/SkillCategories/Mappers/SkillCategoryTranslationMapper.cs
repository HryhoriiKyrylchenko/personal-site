using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Mappers;

public class SkillCategoryTranslationMapper : IMapper<SkillCategoryTranslation, SkillCategoryTranslationDto>
{
    public SkillCategoryTranslationDto MapToDto(SkillCategoryTranslation entity)
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
    
    public List<SkillCategoryTranslationDto> MapToDtoList(
        IEnumerable<SkillCategoryTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}