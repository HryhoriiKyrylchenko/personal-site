namespace PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;

public class CreateResumeCommandValidator : AbstractValidator<CreateResumeCommand>
{
    public CreateResumeCommandValidator()
    {
        RuleFor(x => x.FileUrl)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.FileName)
            .NotEmpty();
    }
}
