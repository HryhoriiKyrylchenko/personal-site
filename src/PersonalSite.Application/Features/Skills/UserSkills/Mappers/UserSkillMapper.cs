namespace PersonalSite.Application.Features.Skills.UserSkills.Mappers;

public static class UserSkillMapper
{
    public static UserSkillDto MapToDto(UserSkill entity, string languageCode)
    {
        return new UserSkillDto
        {
            Id = entity.Id,
            Skill = SkillMapper.MapToDto(entity.Skill, languageCode),
            Proficiency = entity.Proficiency,
        };
    }
    
    public static List<UserSkillDto> MapToDtoList(IEnumerable<UserSkill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public static UserSkillAdminDto MapToAdminDto(UserSkill entity)
    {
        return new UserSkillAdminDto
        {
            Id = entity.Id,
            Skill = SkillMapper.MapToAdminDto(entity.Skill),
            Proficiency = entity.Proficiency,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static List<UserSkillAdminDto> MapToAdminDtoList(IEnumerable<UserSkill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}