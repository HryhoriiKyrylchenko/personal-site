namespace PersonalSite.Application.Services.Skills.Requests;

public class LearningSkillAddRequest
{
    public Guid SkillId { get; set; }
    public short DisplayOrder { get; set; }
}