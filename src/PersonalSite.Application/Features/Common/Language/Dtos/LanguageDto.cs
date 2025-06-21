namespace PersonalSite.Application.Features.Common.Language.Dtos;

public class LanguageDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}