namespace PersonalSite.Application.Features.Skills.SkillCategories.Mappers;

public static class SkillCategoryTranslationMapper
{
    public static SkillCategoryTranslationDto MapToDto(SkillCategoryTranslation entity)
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
    
    public static List<SkillCategoryTranslationDto> MapToDtoList(
        IEnumerable<SkillCategoryTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}