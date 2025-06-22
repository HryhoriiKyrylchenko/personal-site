using PersonalSite.Application.Features.Common.Language.Dtos;

namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguageById;

public record GetLanguageByIdQuery(Guid Id) : IRequest<Result<LanguageDto>>;