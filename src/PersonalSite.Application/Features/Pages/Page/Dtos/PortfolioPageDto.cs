using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Dtos;

public class PortfolioPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<ProjectDto> Projects { get; set; } = null!;
}