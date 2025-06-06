namespace PersonalSite.Application.Services.Translations;

public class SkillCategoryTranslationService : 
    CrudServiceBase<SkillCategoryTranslation, SkillCategoryTranslationDto, SkillCategoryTranslationAddRequest, SkillCategoryTranslationUpdateRequest>, 
    ISkillCategoryTranslationService
{
    public SkillCategoryTranslationService(
        IRepository<SkillCategoryTranslation> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }

    public override async Task<SkillCategoryTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<SkillCategoryTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(SkillCategoryTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(SkillCategoryTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}