using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;

public class GetProjectsHandler : IRequestHandler<GetProjectsQuery, PaginatedResult<ProjectAdminDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetProjectsHandler> _logger;
    private readonly IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto> _projectMapper;

    public GetProjectsHandler(
        IProjectRepository projectRepository,
        ILogger<GetProjectsHandler> logger,
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
            var query = _projectRepository.GetQueryable()
                .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
                .Include(p => p.ProjectSkills)
                    .ThenInclude(ps => ps.Skill)
                        .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                            .ThenInclude(t => t.Language)
                .Include(p => p.ProjectSkills)
                    .ThenInclude(ps => ps.Skill)
                        .ThenInclude(s => s.Category)
                            .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                                .ThenInclude(t => t.Language)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SlugFilter))
                query = query.Where(p => p.Slug.Contains(request.SlugFilter));

            var total = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = _projectMapper.MapToAdminDtoList(entities);

            return PaginatedResult<ProjectAdminDto>.Success(items, total, request.Page, request.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting projects.");
            return PaginatedResult<ProjectAdminDto>.Failure("Error getting projects.");       
        }
    }
}