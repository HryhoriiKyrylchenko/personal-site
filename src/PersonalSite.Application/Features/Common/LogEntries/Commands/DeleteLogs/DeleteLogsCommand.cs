using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.LogEntries.Commands.DeleteLogs;

public record DeleteLogsCommand(List<Guid> Ids) : IRequest<Result>;
