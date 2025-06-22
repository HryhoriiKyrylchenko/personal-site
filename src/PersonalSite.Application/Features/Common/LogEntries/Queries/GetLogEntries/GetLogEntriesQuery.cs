using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;

public record GetLogEntriesQuery(
    int Page = 1,
    int PageSize = 20,
    string? LevelFilter = null,
    string? SourceContextFilter = null,
    DateTime? From = null,
    DateTime? To = null
) : IRequest<PaginatedResult<LogEntryDto>>;