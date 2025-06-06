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
        LanguageContext language) 
        : base(repository, unitOfWork)
    {
        _learningSkillRepository = repository;
        _language = language;
    }


    public async Task<List<LearningSkillDto>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default)
    {
        var learningSkills = await _learningSkillRepository.GetAllOrderedAsync(cancellationToken);
        
        return EntityToDtoMapper.MapLearningSkillsToDtoList(learningSkills, _language.LanguageCode);
    }

    public override async Task<LearningSkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<LearningSkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(LearningSkillAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(LearningSkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}