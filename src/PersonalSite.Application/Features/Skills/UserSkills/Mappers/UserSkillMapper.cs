namespace PersonalSite.Application.Features.Skills.UserSkills.Mappers;

public class UserSkillMapper 
    : ITranslatableMapper<UserSkill, UserSkillDto>, 
        IAdminMapper<UserSkill, UserSkillAdminDto>
{
    private readonly ITranslatableMapper<Skill, SkillDto> _skillMapper;
    private readonly IAdminMapper<Skill, SkillAdminDto> _skillAdminMapper;
    
    public UserSkillMapper(
        ITranslatableMapper<Skill, SkillDto> skillMapper,
        IAdminMapper<Skill, SkillAdminDto> skillAdminMapper)
    {
        _skillMapper = skillMapper;
        _skillAdminMapper = skillAdminMapper;
    }
    
    public UserSkillDto MapToDto(UserSkill entity, string languageCode)
    {
        return new UserSkillDto
        {
            Id = entity.Id,
            Skill = _skillMapper.MapToDto(entity.Skill, languageCode),
            Proficiency = entity.Proficiency,
        };
    }
    
    public List<UserSkillDto> MapToDtoList(IEnumerable<UserSkill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public UserSkillAdminDto MapToAdminDto(UserSkill entity)
    {
        return new UserSkillAdminDto
        {
            Id = entity.Id,
            Skill = _skillAdminMapper.MapToAdminDto(entity.Skill),
            Proficiency = entity.Proficiency,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public List<UserSkillAdminDto> MapToAdminDtoList(IEnumerable<UserSkill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}