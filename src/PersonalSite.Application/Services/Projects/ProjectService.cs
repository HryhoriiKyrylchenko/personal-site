namespace PersonalSite.Application.Services.Projects;

public class ProjectService : 
    CrudServiceBase<Project, ProjectDto, ProjectAddRequest, ProjectUpdateRequest>, 
    IProjectService
{
    private readonly LanguageContext _language;
    private readonly IProjectRepository _projectRepository;
    
    public ProjectService(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language) 
        : base(repository, unitOfWork)
    {
        _language = language;
        _projectRepository = repository;
    }


    public async Task<ProjectDto?> GetProjectAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetWithFullDataAsync(id, cancellationToken);
        
        return project == null ? null : EntityToDtoMapper.MapProjectToDto(project, _language.LanguageCode);
    }

    public async Task<ProjectDto?> GetLastProjectAsync(CancellationToken cancellationToken = default)
    {
        var lastProject = await _projectRepository.GetLastAsync(cancellationToken);
        
        return lastProject == null ? null : EntityToDtoMapper.MapProjectToDto(lastProject, _language.LanguageCode);
    }

    public async Task<List<ProjectDto>> GetProjectsFullDataAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetAllWithFullDataAsync(cancellationToken);

        return EntityToDtoMapper.MapProjectsToDtoList(projects, _language.LanguageCode);
    }

    public override async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(ProjectAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(ProjectUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}