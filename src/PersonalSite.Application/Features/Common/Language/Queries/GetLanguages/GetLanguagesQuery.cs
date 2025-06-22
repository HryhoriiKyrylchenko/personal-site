using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguages;

public record GetLanguagesQuery() : IRequest<Result<List<LanguageDto>>>;