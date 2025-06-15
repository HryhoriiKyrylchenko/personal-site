namespace PersonalSite.Application.Services.Skills;

public class SkillService : 
    CrudServiceBase<Skill, SkillDto, SkillAddRequest, SkillUpdateRequest>, 
    ISkillService
{
    private readonly LanguageContext _language;
    private readonly ISkillRepository _skillRepository;
    
    public SkillService(
        ISkillRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language,
        ILogger<CrudServiceBase<Skill, SkillDto, SkillAddRequest, SkillUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _skillRepository = repository;
        _language = language;
    }

    public override async Task<SkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var skill = await _skillRepository.GetWithTranslationsById(id, cancellationToken);
        
        return skill == null ? null : EntityToDtoMapper.MapSkillToDto(skill, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skills = await _skillRepository.GetAllOrderedAsync(cancellationToken);
        
        return EntityToDtoMapper.MapSkillsToDtoList(skills, _language.LanguageCode);   
    }

    public override async Task AddAsync(SkillAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newSkill = new Skill()
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            Key = request.Key
        };
        
        await _skillRepository.AddAsync(newSkill, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(SkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingSkill = await _skillRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingSkill is null) throw new Exception("Skill not found");
        
        existingSkill.CategoryId = request.CategoryId;
        existingSkill.Key = request.Key;
        
        await _skillRepository.UpdateAsync(existingSkill, cancellationToken);
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