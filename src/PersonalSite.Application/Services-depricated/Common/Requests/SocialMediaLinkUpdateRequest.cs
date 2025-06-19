namespace PersonalSite.Application.Services.Common.Requests;

public class SocialMediaLinkUpdateRequest
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}