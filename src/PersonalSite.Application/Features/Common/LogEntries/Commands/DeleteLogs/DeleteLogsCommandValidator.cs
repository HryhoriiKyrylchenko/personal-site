namespace PersonalSite.Application.Features.Common.LogEntries.Commands.DeleteLogs;

public class DeleteLogsCommandValidator : AbstractValidator<DeleteLogsCommand>
{
    public DeleteLogsCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("At least one log ID must be provided.");
    }
}