namespace PersonalSite.Application.Services.Translations.Requests;

public class LanguageUpdateRequest
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}