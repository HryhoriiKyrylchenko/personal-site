using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Entities.Projects;

[Table("Projects")]
public class Project : SoftDeletableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(100)] 
    public string Slug { get; set; } = string.Empty;
    [MaxLength(255)]
    public string CoverImage { get; set; } = string.Empty;
    [MaxLength(255)]
    public string DemoUrl { get; set; }  = string.Empty;
    [MaxLength(255)]
    public string RepoUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<ProjectTranslation> Translations { get; set; } = [];
    public ICollection<ProjectSkill> ProjectSkills { get; set; } = [];
}