namespace PersonalSite.Application.Features.Skills.LearningSkills.Dtos;

public class LearningSkillDto
{
    public Guid Id { get; set; }
    public SkillDto Skill { get; set; } = null!;
    public string LearningStatus { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }
}