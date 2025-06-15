namespace PersonalSite.Application.Services.Common.Validators;

public class ResumeAddRequestValidator : AbstractValidator<ResumeAddRequest>
{
    public ResumeAddRequestValidator()
    {
        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("FileUrl is required.")
            .MaximumLength(255).WithMessage("FileUrl must be 255 characters or fewer.");

        RuleFor(x => x.FileName)
            .MaximumLength(255).WithMessage("FileName must be 255 characters or fewer.");
    }
}