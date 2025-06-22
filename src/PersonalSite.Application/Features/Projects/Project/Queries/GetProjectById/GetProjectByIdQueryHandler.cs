using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectAdminDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetProjectByIdQueryHandler> _logger;
    private readonly IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto> _mapper;
    
    public GetProjectByIdQueryHandler(
        IProjectRepository projectRepository,
        ILogger<GetProjectByIdQueryHandler> logger,
        IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto> mapper)
    {
        _projectRepository = projectRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<ProjectAdminDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _projectRepository.GetWithFullDataAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Project not found.");   
                return Result<ProjectAdminDto>.Failure("Project not found.");
            }
            var dto = _mapper.MapToAdminDto(entity);
            return Result<ProjectAdminDto>.Success(dto);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting project by id.");
            return Result<ProjectAdminDto>.Failure("Error getting project by id.");      
        }
    }
}