namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;

public record GetProjectsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SlugFilter = null
) : IRequest<PaginatedResult<ProjectAdminDto>>;