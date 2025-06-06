namespace PersonalSite.Application.Services.Skills;

public class UserSkillService : 
    CrudServiceBase<UserSkill, UserSkillDto, UserSkillAddRequest, UserSkillUpdateRequest>, 
    IUserSkillService
{
    private readonly LanguageContext _language;
    private readonly IUserSkillRepository _userSkillRepository;
    
    public UserSkillService(
        IUserSkillRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language) 
        : base(repository, unitOfWork)
    {
        _userSkillRepository = repository;
        _language = language;
    }

    public async Task<List<SkillDto>> GetAllAsSkillAsync(CancellationToken cancellationToken = default)
    {
        var skills = await _userSkillRepository.GetAllActiveAsync(cancellationToken);

        return EntityToDtoMapper.MapSkillsToDtoList(skills.Select(us => us.Skill), _language.LanguageCode);
    }

    public async Task<List<UserSkillDto>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default)
    {
        var skills = await _userSkillRepository.GetAllActiveAsync(cancellationToken);

        return EntityToDtoMapper.MapUserSkillsToDtoList(skills, _language.LanguageCode);
    }

    public override async Task<UserSkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<UserSkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(UserSkillAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(UserSkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}