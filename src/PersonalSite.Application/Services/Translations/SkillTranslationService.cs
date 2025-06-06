namespace PersonalSite.Application.Services.Translations;

public class SkillTranslationService : 
    CrudServiceBase<SkillTranslation, SkillTranslationDto, SkillTranslationAddRequest, SkillTranslationUpdateRequest>, 
    ISkillTranslationService
{
    public SkillTranslationService(
        IRepository<SkillTranslation> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }

    public override async Task<SkillTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<SkillTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(SkillTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(SkillTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}