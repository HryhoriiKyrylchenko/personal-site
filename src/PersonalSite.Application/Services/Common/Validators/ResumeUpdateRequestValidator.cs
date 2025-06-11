namespace PersonalSite.Application.Services.Common.Validators;

public class ResumeUpdateRequestValidator : AbstractValidator<ResumeUpdateRequest>
{
    public ResumeUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Resume ID is required.");

        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("FileUrl is required.")
            .MaximumLength(255).WithMessage("FileUrl must be 255 characters or fewer.");

        RuleFor(x => x.FileName)
            .MaximumLength(255).WithMessage("FileName must be 255 characters or fewer.");
    }
}