namespace PersonalSite.Application.Services.Skills;

public class ProjectSkillService : 
    CrudServiceBase<ProjectSkill, SkillDto, ProjectSkillAddRequest, ProjectSkillUpdateRequest>, 
    IProjectSkillService
{
    public ProjectSkillService(
        IRepository<ProjectSkill> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }

    public override async Task<SkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(ProjectSkillAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(ProjectSkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}