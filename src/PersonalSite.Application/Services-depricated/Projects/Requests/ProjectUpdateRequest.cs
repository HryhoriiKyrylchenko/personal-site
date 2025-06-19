namespace PersonalSite.Application.Services.Projects.Requests;

public class ProjectUpdateRequest
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string DemoUrl { get; set; }  = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
}