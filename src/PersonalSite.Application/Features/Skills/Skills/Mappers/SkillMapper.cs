namespace PersonalSite.Application.Features.Skills.Skills.Mappers;

public static class SkillMapper
{
    public static SkillDto MapToDto(Skill entity, string languageCode)
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
            Category = SkillCategoryMapper.MapToDto(entity.Category, languageCode)
        };
    }
    
    public static List<SkillDto> MapToDtoList(IEnumerable<Skill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public static SkillAdminDto MapToAdminDto(Skill entity)
    {
        return new SkillAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Translations = SkillTranslationMapper.MapToDtoList(entity.Translations),
            Category = SkillCategoryMapper.MapToAdminDto(entity.Category)
        };
    }

    public static List<SkillAdminDto> MapToAdminDtoList(IEnumerable<Skill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();   
    }
}