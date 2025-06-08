namespace PersonalSite.Application.Services.Pages.Requests;

public class PageUpdateRequest
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
}