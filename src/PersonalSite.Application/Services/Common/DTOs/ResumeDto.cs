namespace PersonalSite.Application.Services.Common.DTOs;

public class ResumeDto
{
    public Guid Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public bool IsActive { get; set; }
}