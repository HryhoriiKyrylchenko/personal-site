using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Features.Skills.Skills.Mappers;

public class SkillTranslationMapper : IMapper<SkillTranslation, SkillTranslationDto>
{
    public SkillTranslationDto MapToDto(SkillTranslation entity)
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
    
    public List<SkillTranslationDto> MapToDtoList(
        IEnumerable<SkillTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}