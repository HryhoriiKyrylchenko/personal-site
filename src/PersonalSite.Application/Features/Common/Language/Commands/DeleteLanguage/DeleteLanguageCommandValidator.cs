namespace PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;

public class DeleteLanguageCommandValidator : AbstractValidator<DeleteLanguageCommand>
{
    public DeleteLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Language ID is required.");
    }
}