using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Projects.Project.Dtos;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string DemoUrl { get; set; }  = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public Dictionary<string, string> DescriptionSections { get; set; } = [];
    
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
    
    public List<ProjectSkillDto> Skills { get; set; } = [];
}