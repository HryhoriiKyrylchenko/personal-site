using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Application.Features.Projects.Project.Mappers;

public class ProjectSkillMapper 
    : ITranslatableMapper<ProjectSkill, ProjectSkillDto>,
    IAdminMapper<ProjectSkill, ProjectSkillAdminDto>
{
    private readonly ITranslatableMapper<Skill, SkillDto> _skillMapper;
    private readonly IAdminMapper<Skill, SkillAdminDto> _skillAdminMapper;
    public ProjectSkillMapper(
        ITranslatableMapper<Skill, SkillDto> skillMapper,
        IAdminMapper<Skill, SkillAdminDto> skillAdminMapper)
    {
        _skillMapper = skillMapper;
        _skillAdminMapper = skillAdminMapper;
    }
    
    public ProjectSkillDto MapToDto(ProjectSkill entity, string languageCode)
    {
        return new ProjectSkillDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Skill = _skillMapper.MapToDto(entity.Skill, languageCode)
        };
    }
    
    public List<ProjectSkillDto> MapToDtoList(IEnumerable<ProjectSkill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }

    public ProjectSkillAdminDto MapToAdminDto(ProjectSkill entity)
    {
        return new ProjectSkillAdminDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Skill = _skillAdminMapper.MapToAdminDto(entity.Skill)
        };
    }

    public List<ProjectSkillAdminDto> MapToAdminDtoList(IEnumerable<ProjectSkill> entities)
    {
        return entities.Select(MapToAdminDto).ToList();   
    }
}