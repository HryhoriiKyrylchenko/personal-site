namespace PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;

public record DeleteLanguageCommand(Guid Id) : IRequest<Result>;