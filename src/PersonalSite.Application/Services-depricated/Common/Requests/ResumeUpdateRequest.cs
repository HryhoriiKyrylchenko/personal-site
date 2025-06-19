namespace PersonalSite.Application.Services.Common.Requests;

public class ResumeUpdateRequest
{
    public Guid Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}