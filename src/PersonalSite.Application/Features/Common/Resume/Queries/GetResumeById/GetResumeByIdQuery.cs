using PersonalSite.Application.Features.Common.Resume.Dtos;

namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;

public record GetResumeByIdQuery(Guid Id) : IRequest<Result<ResumeDto>>;