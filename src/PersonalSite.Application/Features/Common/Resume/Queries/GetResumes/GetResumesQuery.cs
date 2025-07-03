using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;

public record GetResumesQuery(int Page = 1, int PageSize = 20, bool? IsActive = null)
    : IRequest<PaginatedResult<ResumeDto>>;
