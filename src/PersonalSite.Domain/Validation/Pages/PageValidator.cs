using PersonalSite.Domain.Entities.Pages;

namespace PersonalSite.Domain.Validation.Pages;

public class PageValidator : AbstractValidator<Page>
{
    public PageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Page ID is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
        
        RuleFor(x => x.PageImage)
            .MaximumLength(255).WithMessage("Page image URL must be 255 characters or fewer.");
    }
}