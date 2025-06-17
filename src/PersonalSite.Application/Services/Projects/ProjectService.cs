using PersonalSite.Application.Features.Projects.Common.Dtos;

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
        LanguageContext language,
        ILogger<CrudServiceBase<Project, ProjectDto, ProjectAddRequest, ProjectUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _language = language;
        _projectRepository = repository;
    }

    public async Task<ProjectDto?> GetLastProjectAsync(CancellationToken cancellationToken = default)
    {
        var lastProject = await _projectRepository.GetLastAsync(cancellationToken);
        
        return lastProject == null ? null : EntityToDtoMapper.MapProjectToDto(lastProject, _language.LanguageCode);
    }

    public override async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetWithFullDataAsync(id, cancellationToken);
        
        return project == null ? null : EntityToDtoMapper.MapProjectToDto(project, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetAllWithFullDataAsync(cancellationToken);

        return EntityToDtoMapper.MapProjectsToDtoList(projects, _language.LanguageCode);
    }

    public override async Task AddAsync(ProjectAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newProject = new Project
        {
            Id = Guid.NewGuid(),
            Slug = request.Slug,
            CoverImage = request.CoverImage,
            DemoUrl = request.DemoUrl,
            RepoUrl = request.RepoUrl,
            CreatedAt = DateTime.UtcNow
        };
        
        await _projectRepository.AddAsync(newProject, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(ProjectUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingProject = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingProject is null) throw new Exception("Project not found");
        
        existingProject.Slug = request.Slug;
        existingProject.CoverImage = request.CoverImage;
        existingProject.DemoUrl = request.DemoUrl;
        existingProject.RepoUrl = request.RepoUrl;
        existingProject.UpdatedAt = DateTime.UtcNow;
        
        await _projectRepository.UpdateAsync(existingProject, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _projectRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _projectRepository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }   
    }
}