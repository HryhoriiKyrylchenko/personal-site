namespace PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;

public class UpdatePageCommandValidator : AbstractValidator<UpdatePageCommand>
{
    public UpdatePageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Page ID is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
        
        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage("At least one translation is required.");
        
        RuleForEach(x => x.Translations).SetValidator(new PageTranslationDtoValidator());
    }
}