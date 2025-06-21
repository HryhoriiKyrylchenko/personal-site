namespace PersonalSite.Application.Features.Skills.LearningSkills.Mappers;

public static class LearningSkillMapper
{
    public static LearningSkillDto MapToDto(LearningSkill entity, string languageCode)
    {
        return new LearningSkillDto
        {
            Id = entity.Id,
            Skill = SkillMapper.MapToDto(entity.Skill, languageCode),
            LearningStatus = entity.LearningStatus.ToString(),
            DisplayOrder = entity.DisplayOrder
        };
    }
    
    public static List<LearningSkillDto> MapToDtoList(IEnumerable<LearningSkill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public static LearningSkillAdminDto MapToAdminDto(LearningSkill entity)
    {
        return new LearningSkillAdminDto
        {
            Id = entity.Id,
            Skill = SkillMapper.MapToAdminDto(entity.Skill),
            LearningStatus = entity.LearningStatus,
            DisplayOrder = entity.DisplayOrder
        };
    }

    public static List<LearningSkillAdminDto> MapToAdminDtoList(IEnumerable<LearningSkill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}