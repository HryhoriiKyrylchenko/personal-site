using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Dtos;

public class HomePageDto
{
    public PageDto PageData { get; set; } = null!;
    public IReadOnlyList<UserSkillDto> UserSkills { get; set; } = null!;
    public ProjectDto? LastProject { get; set; }
}