namespace PersonalSite.Application.Services.Skills;

public class SkillCategoryService : 
    CrudServiceBase<SkillCategory, SkillCategoryDto, SkillCategoryAddRequest, SkillCategoryUpdateRequest>, 
    ISkillCategoryService
{
    public SkillCategoryService(
        IRepository<SkillCategory> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }

    public override async Task<SkillCategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<SkillCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(SkillCategoryAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(SkillCategoryUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}