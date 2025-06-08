namespace PersonalSite.Application.Services.Aggregates.DTOs;

public class PortfolioPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<ProjectDto> Projects { get; set; } = null!;
}