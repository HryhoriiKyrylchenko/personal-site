namespace PersonalSite.Application.Features.Projects.Project.Dtos;

public class ProjectAdminDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string DemoUrl { get; set; }  = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<ProjectTranslationDto> Translations { get; set; } = [];
    public List<SkillAdminDto> Skills { get; set; } = [];
}