namespace PersonalSite.Application.Services.Skills;

public class LearningSkillService : 
    CrudServiceBase<LearningSkill, LearningSkillDto, LearningSkillAddRequest, LearningSkillUpdateRequest>, 
    ILearningSkillService
{
    private readonly LanguageContext _language;
    private readonly ILearningSkillRepository _learningSkillRepository;
    
    public LearningSkillService(
        ILearningSkillRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language,
        ILogger<CrudServiceBase<LearningSkill, LearningSkillDto, LearningSkillAddRequest, LearningSkillUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _learningSkillRepository = repository;
        _language = language;
    }

    public override async Task<LearningSkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var learningSkill = await _learningSkillRepository.GetWithFullDataByIdAsync(id, cancellationToken);
        
        return learningSkill == null ? 
            null : 
            EntityToDtoMapper.MapLearningSkillToDto(learningSkill, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<LearningSkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var learningSkills = await _learningSkillRepository.GetAllOrderedAsync(cancellationToken);
        
        return EntityToDtoMapper.MapLearningSkillsToDtoList(learningSkills, _language.LanguageCode);
    }

    public override async Task AddAsync(LearningSkillAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newLearningSkill = new LearningSkill
        {
            Id = Guid.NewGuid(),
            SkillId = request.SkillId,
            LearningStatus = LearningStatus.Planning,
            DisplayOrder = request.DisplayOrder
        };
        
        await _learningSkillRepository.AddAsync(newLearningSkill, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(LearningSkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingLearningSkill = await _learningSkillRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingLearningSkill is null) throw new Exception("Learning skill not found");
        
        existingLearningSkill.LearningStatus = request.LearningStatus;
        existingLearningSkill.DisplayOrder = request.DisplayOrder;
        
        await _learningSkillRepository.UpdateAsync(existingLearningSkill, cancellationToken);
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