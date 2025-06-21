namespace PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;

public record UpdatePageCommand(
    Guid Id,
    string Key,
    List<PageTranslationDto> Translations
) : IRequest<Result>;