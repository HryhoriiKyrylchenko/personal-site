namespace PersonalSite.Application.Features.Pages.Common.Dtos;

public class HomePageDto
{
    public PageDto PageData { get; set; } = null!;
    public IReadOnlyList<UserSkillDto> UserSkills { get; set; } = null!;
    public ProjectDto? LastProject { get; set; }
}