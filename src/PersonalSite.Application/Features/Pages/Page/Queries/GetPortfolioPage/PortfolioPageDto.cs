namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPortfolioPage;

public class PortfolioPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<ProjectDto> Projects { get; set; } = null!;
}