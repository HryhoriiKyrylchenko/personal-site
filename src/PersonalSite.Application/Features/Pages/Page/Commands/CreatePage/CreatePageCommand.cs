using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;

public record CreatePageCommand(
    string Key,
    List<PageTranslationDto> Translations
) : IRequest<Result<Guid>>;