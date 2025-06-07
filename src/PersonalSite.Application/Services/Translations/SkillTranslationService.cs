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
        var skillTranslation = await Repository.GetByIdAsync(id, cancellationToken);
        return skillTranslation == null
            ? null
            : EntityToDtoMapper.MapSkillTranslationToDto(skillTranslation);
    }

    public override async Task<IReadOnlyList<SkillTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skillTranslations = await Repository.ListAsync(cancellationToken);
        
        return EntityToDtoMapper.MapSkillTranslationsToDtoList(skillTranslations);  
    }

    public override async Task AddAsync(SkillTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        var newSkillTranslation = new SkillTranslation
        {
            Id = Guid.NewGuid(),
            LanguageId = request.LanguageId,
            SkillId = request.SkillId,
            Name = request.Name,
            Description = request.Description
        };
        
        await Repository.AddAsync(newSkillTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(SkillTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingSkillTranslation = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingSkillTranslation is null) throw new Exception("Skill translation not found");
        
        existingSkillTranslation.Name = request.Name;
        existingSkillTranslation.Description = request.Description;
        
        Repository.Update(existingSkillTranslation);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            Repository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        } 
    }
}