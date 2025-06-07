namespace PersonalSite.Application.Services.Projects.Requests;

public class ProjectAddRequest
{
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string DemoUrl { get; set; }  = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
}