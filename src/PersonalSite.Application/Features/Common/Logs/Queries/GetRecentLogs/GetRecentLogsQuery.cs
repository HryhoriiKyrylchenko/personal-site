using PersonalSite.Application.Features.Common.Logs.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Logs.Queries.GetRecentLogs;

public record GetRecentLogsQuery(int Count) : IRequest<Result<List<LogEntryDto>>>;