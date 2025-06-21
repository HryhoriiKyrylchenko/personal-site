namespace PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;

public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
{
    public UpdateLanguageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Language ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(2).WithMessage("Code must be 2 characters or fewer.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}