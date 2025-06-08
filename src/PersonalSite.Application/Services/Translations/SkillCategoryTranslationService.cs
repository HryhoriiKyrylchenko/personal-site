namespace PersonalSite.Application.Services.Translations;

public class SkillCategoryTranslationService : 
    CrudServiceBase<SkillCategoryTranslation, SkillCategoryTranslationDto, SkillCategoryTranslationAddRequest, SkillCategoryTranslationUpdateRequest>, 
    ISkillCategoryTranslationService
{
    ISkillCategoryTranslationRepository _skillCategoryTranslationRepository;
    
    public SkillCategoryTranslationService(
        ISkillCategoryTranslationRepository repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
        _skillCategoryTranslationRepository = repository;  
    }

    public override async Task<SkillCategoryTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var skillCategoryTranslation = await _skillCategoryTranslationRepository.GetWithLanguageByIdAsync(id, cancellationToken);
        return skillCategoryTranslation == null
            ? null
            : EntityToDtoMapper.MapSkillCategoryTranslationToDto(skillCategoryTranslation);
    }

    public override async Task<IReadOnlyList<SkillCategoryTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skillCategoryTranslations = await _skillCategoryTranslationRepository.ListWithLanguageAsync(cancellationToken);
        
        return EntityToDtoMapper.MapSkillCategoryTranslationsToDtoList(skillCategoryTranslations);   
    }

    public override async Task AddAsync(SkillCategoryTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        var newSkillCategoryTranslation = new SkillCategoryTranslation
        {
            Id = Guid.NewGuid(),
            LanguageId = request.LanguageId,
            SkillCategoryId = request.SkillCategoryId,
            Name = request.Name,
            Description = request.Description
        };
        
        await Repository.AddAsync(newSkillCategoryTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);  
    }

    public override async Task UpdateAsync(SkillCategoryTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingSkillCategoryTranslation = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingSkillCategoryTranslation is null) throw new Exception("Skill category translation not found");
        
        existingSkillCategoryTranslation.Name = request.Name;
        existingSkillCategoryTranslation.Description = request.Description;
        
        Repository.Update(existingSkillCategoryTranslation);
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