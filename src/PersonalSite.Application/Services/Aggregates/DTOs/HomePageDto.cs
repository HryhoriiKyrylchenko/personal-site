namespace PersonalSite.Application.Services.Aggregates.DTOs;

public class HomePageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<UserSkillDto> UserSkills { get; set; } = null!;
    public ProjectDto? LastProject { get; set; } = null!;
}