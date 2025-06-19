namespace PersonalSite.Application.Services.Skills.Requests;

public class LearningSkillUpdateRequest
{
    public Guid Id { get; set; }
    public LearningStatus LearningStatus { get; set; }
    public short DisplayOrder { get; set; }
}