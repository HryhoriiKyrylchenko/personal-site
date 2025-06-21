namespace PersonalSite.Application.Features.Pages.Page.Dtos;

public class AboutPageDto
{
    public PageDto PageData { get; set; } = null!;
    public IReadOnlyList<UserSkillDto> UserSkills { get; set; } = null!;
    public IReadOnlyList<LearningSkillDto> LearningSkills { get; set; } = null!;
}