using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Logs.Commands.DeleteOldLogs;

public record DeleteOldLogsCommand(DateTime Cutoff) : IRequest<Result<int>>;