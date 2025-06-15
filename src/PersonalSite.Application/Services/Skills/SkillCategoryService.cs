namespace PersonalSite.Application.Services.Skills;

public class SkillCategoryService : 
    CrudServiceBase<SkillCategory, SkillCategoryDto, SkillCategoryAddRequest, SkillCategoryUpdateRequest>, 
    ISkillCategoryService
{
    private readonly LanguageContext _language;
    private readonly ISkillCategoryRepository _skillCategoryRepository;
    
    public SkillCategoryService(
        ISkillCategoryRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language,
        ILogger<CrudServiceBase<SkillCategory, SkillCategoryDto, SkillCategoryAddRequest, SkillCategoryUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _skillCategoryRepository = repository;
        _language = language;
    }

    public override async Task<SkillCategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var skillCategory = await _skillCategoryRepository.GetWithTranslationsByIdAsync(id, cancellationToken);
        
        return skillCategory == null ? null : EntityToDtoMapper.MapSkillCategoryToDto(skillCategory, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<SkillCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skillCategories = await _skillCategoryRepository.GetAllOrderedAsync(cancellationToken);
        
        return EntityToDtoMapper.MapSkillCategoriesToDtoList(skillCategories, _language.LanguageCode);
    }

    public override async Task AddAsync(SkillCategoryAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newSkillCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Key = request.Key,
            DisplayOrder = request.DisplayOrder
        };
        
        await _skillCategoryRepository.AddAsync(newSkillCategory, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(SkillCategoryUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingSkillCategory = await _skillCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingSkillCategory is null) throw new Exception("Skill category not found");
        
        existingSkillCategory.Key = request.Key;
        existingSkillCategory.DisplayOrder = request.DisplayOrder;
        
        await _skillCategoryRepository.UpdateAsync(existingSkillCategory, cancellationToken);
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