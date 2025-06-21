namespace PersonalSite.Application.Features.Skills.SkillCategories.Mappers;

public static class SkillCategoryMapper
{
    public static SkillCategoryDto MapToDto(SkillCategory entity, string languageCode)
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

    public static List<SkillCategoryDto> MapToDtoList(IEnumerable<SkillCategory> entities,
        string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public static SkillCategoryAdminDto MapToAdminDto(SkillCategory entity)
    {
        return new SkillCategoryAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            DisplayOrder = entity.DisplayOrder,
            Translations = SkillCategoryTranslationMapper.MapToDtoList(entity.Translations)
        };
    }

    public static List<SkillCategoryAdminDto> MapToAdminDtoList(IEnumerable<SkillCategory> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}