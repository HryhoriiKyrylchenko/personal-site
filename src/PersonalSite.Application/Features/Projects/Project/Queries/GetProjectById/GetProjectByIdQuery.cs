using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IRequest<Result<ProjectAdminDto>>;
