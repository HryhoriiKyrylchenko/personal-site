namespace PersonalSite.Application.Features.Common.Language.Commands.CreateLanguage;

public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
{
    public CreateLanguageCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(2).WithMessage("Code must be 2 characters or fewer.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}