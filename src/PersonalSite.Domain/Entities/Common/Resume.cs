namespace PersonalSite.Domain.Entities.Common;

[Table("Resumes")]
public class Resume
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(255)]
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public bool IsActive { get; set; }
}