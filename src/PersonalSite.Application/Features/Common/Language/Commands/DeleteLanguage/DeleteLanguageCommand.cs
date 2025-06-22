using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;

public record DeleteLanguageCommand(Guid Id) : IRequest<Result>;