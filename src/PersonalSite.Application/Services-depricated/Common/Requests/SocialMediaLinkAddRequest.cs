namespace PersonalSite.Application.Services.Common.Requests;

public class SocialMediaLinkAddRequest
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}