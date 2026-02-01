using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;

public record UpdatePageCommand(
    Guid Id,
    string Key,
    string PageImage,
    List<PageTranslationDto> Translations
) : IRequest<Result>;