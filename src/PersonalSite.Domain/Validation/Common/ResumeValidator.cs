using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Validation.Common;

public class ResumeValidator : AbstractValidator<Resume>
{
    public ResumeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Resume ID is required.");

        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("FileUrl is required.")
            .MaximumLength(255).WithMessage("FileUrl must be 255 characters or fewer.");

        RuleFor(x => x.FileName)
            .MaximumLength(255).WithMessage("FileName must be 255 characters or fewer.");

        RuleFor(x => x.UploadedAt)
            .NotEmpty().WithMessage("UploadedAt is required.")
            .LessThanOrEqualTo(_ => DateTime.UtcNow).WithMessage("UploadedAt cannot be in the future.");
    }
}