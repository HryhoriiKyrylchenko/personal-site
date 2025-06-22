namespace PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;

public class CreatePageCommandValidator : AbstractValidator<CreatePageCommand>
{
    public CreatePageCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
        
        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage("At least one translation is required.");
        
        RuleForEach(x => x.Translations).SetValidator(new PageTranslationDtoValidator());
    }
}