using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Dtos;

public class LearningSkillAdminDto
{
    public Guid Id { get; set; }
    public SkillAdminDto Skill { get; set; } = null!;   
    public LearningStatus LearningStatus { get; set; }
    public short DisplayOrder { get; set; }
}