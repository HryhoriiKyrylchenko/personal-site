namespace PersonalSite.Application.Features.Pages.Common.Dtos;

public class PortfolioPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<ProjectDto> Projects { get; set; } = null!;
}