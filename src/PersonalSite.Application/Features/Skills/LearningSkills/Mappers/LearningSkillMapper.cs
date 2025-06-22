using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Mappers;

public class LearningSkillMapper 
    : ITranslatableMapper<LearningSkill, LearningSkillDto>,
        IAdminMapper<LearningSkill, LearningSkillAdminDto>
{
    private readonly ITranslatableMapper<Skill, SkillDto> _skillMapper;
    private readonly IAdminMapper<Skill, SkillAdminDto> _skillAdminMapper;
    
    public LearningSkillMapper(
        ITranslatableMapper<Skill, SkillDto> skillMapper,
        IAdminMapper<Skill, SkillAdminDto> skillAdminMapper)
    {
        _skillMapper = skillMapper;   
        _skillAdminMapper = skillAdminMapper;
    }
    
    public LearningSkillDto MapToDto(LearningSkill entity, string languageCode)
    {
        return new LearningSkillDto
        {
            Id = entity.Id,
            Skill = _skillMapper.MapToDto(entity.Skill, languageCode),
            LearningStatus = entity.LearningStatus.ToString(),
            DisplayOrder = entity.DisplayOrder
        };
    }
    
    public List<LearningSkillDto> MapToDtoList(IEnumerable<LearningSkill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public LearningSkillAdminDto MapToAdminDto(LearningSkill entity)
    {
        return new LearningSkillAdminDto
        {
            Id = entity.Id,
            Skill = _skillAdminMapper.MapToAdminDto(entity.Skill),
            LearningStatus = entity.LearningStatus,
            DisplayOrder = entity.DisplayOrder
        };
    }

    public List<LearningSkillAdminDto> MapToAdminDtoList(IEnumerable<LearningSkill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}