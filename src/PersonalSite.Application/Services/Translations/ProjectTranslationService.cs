namespace PersonalSite.Application.Services.Translations;

public class ProjectTranslationService : 
    CrudServiceBase<ProjectTranslation, ProjectTranslationDto, ProjectTranslationAddRequest, ProjectTranslationUpdateRequest>, 
    IProjectTranslationService
{
    public ProjectTranslationService(
        IRepository<ProjectTranslation> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }

    public override async Task<ProjectTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<ProjectTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(ProjectTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(ProjectTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}