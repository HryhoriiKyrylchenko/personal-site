using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.Common.Dtos;

public class LearningSkillDto
{
    public Guid Id { get; set; }
    public SkillDto Skill { get; set; } = null!;
    public string LearningStatus { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }
}