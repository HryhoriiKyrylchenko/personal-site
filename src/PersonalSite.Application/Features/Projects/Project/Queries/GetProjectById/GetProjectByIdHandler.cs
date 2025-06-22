using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetProjectByIdHandler> _logger;
    private readonly IMapper<Domain.Entities.Projects.Project, ProjectDto> _mapper;
    
    public GetProjectByIdHandler(
        IProjectRepository projectRepository,
        ILogger<GetProjectByIdHandler> logger,
        IMapper<Domain.Entities.Projects.Project, ProjectDto> mapper)
    {
        _projectRepository = projectRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _projectRepository.GetWithFullDataAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Project not found.");   
                return Result<ProjectDto>.Failure("Project not found.");
            }
            var dto = _mapper.MapToDto(entity);
            return Result<ProjectDto>.Success(dto);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting project by id.");
            return Result<ProjectDto>.Failure("Error getting project by id.");      
        }
    }
}