namespace PersonalSite.Application.Features.Common.Resume.Commands.DeleteResume;

public class DeleteResumeCommandValidator : AbstractValidator<DeleteResumeCommand>
{
    public DeleteResumeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}