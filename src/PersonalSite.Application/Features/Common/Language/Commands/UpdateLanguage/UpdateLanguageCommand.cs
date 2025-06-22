namespace PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;

public record UpdateLanguageCommand(Guid Id, string Code, string Name) : IRequest<Result>;