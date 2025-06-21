namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguages;

public record GetLanguagesQuery() : IRequest<Result<List<LanguageDto>>>;