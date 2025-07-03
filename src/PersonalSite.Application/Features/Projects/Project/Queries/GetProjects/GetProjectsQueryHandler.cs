using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, PaginatedResult<ProjectAdminDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetProjectsQueryHandler> _logger;
    private readonly IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto> _projectMapper;

    public GetProjectsQueryHandler(
        IProjectRepository projectRepository,
        ILogger<GetProjectsQueryHandler> logger,
        IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto> projectMapper)
    {
        _projectRepository = projectRepository;
        _logger = logger;
        _projectMapper = projectMapper;       
    }

    public async Task<PaginatedResult<ProjectAdminDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var projects = await _projectRepository.GetFilteredAsync(
                request.Page,
                request.PageSize,
                request.SlugFilter,
                cancellationToken);

            if (projects.IsFailure || projects.Value == null)
            {
                _logger.LogWarning("Projects not found.");   
                return PaginatedResult<ProjectAdminDto>.Failure("Projects not found.");
            }

            var items = _projectMapper.MapToAdminDtoList(projects.Value);

            return PaginatedResult<ProjectAdminDto>.Success(items, projects.PageNumber, projects.PageSize, projects.TotalCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting projects.");
            return PaginatedResult<ProjectAdminDto>.Failure("Error getting projects.");       
        }
    }
}