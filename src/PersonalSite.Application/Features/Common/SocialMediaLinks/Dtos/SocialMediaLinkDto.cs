namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;

public class SocialMediaLinkDto
{
    public Guid Id { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}