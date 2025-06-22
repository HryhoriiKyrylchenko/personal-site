namespace PersonalSite.Application.Features.Common.Language.Commands.CreateLanguage;

public record CreateLanguageCommand(string Code, string Name) : IRequest<Result<Guid>>;
