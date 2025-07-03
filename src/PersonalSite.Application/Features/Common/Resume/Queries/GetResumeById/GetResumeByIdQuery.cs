using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;

public record GetResumeByIdQuery(Guid Id) : IRequest<Result<ResumeDto>>;