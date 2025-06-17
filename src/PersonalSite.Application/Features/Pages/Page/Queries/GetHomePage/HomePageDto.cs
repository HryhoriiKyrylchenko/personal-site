using PersonalSite.Application.Features.Projects.Common.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;

public class HomePageDto
{
    public PageDto PageData { get; set; } = null!;
    public IReadOnlyList<UserSkillDto> UserSkills { get; set; } = null!;
    public ProjectDto? LastProject { get; set; }
}