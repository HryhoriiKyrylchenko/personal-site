namespace PersonalSite.Application.Features.Projects.Project.Mappers;

public static class ProjectSkillMapper
{
    public static ProjectSkillDto MapToDto(ProjectSkill entity, string languageCode)
    {
        return new ProjectSkillDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            Skill = SkillMapper.MapToDto(entity.Skill, languageCode)
        };
    }
    
    public static List<ProjectSkillDto> MapToDtoList(IEnumerable<ProjectSkill> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
}