namespace PersonalSite.Application.Services.Skills;

public class SkillService : 
    CrudServiceBase<Skill, SkillDto, SkillAddRequest, SkillUpdateRequest>, 
    ISkillService
{
    public SkillService(
        IRepository<Skill> repository, 
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

    public override async Task AddAsync(SkillAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(SkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}