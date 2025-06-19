using PersonalSite.Application.Features.Skills.Common.Dtos;

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
        LanguageContext language,
        ILogger<CrudServiceBase<UserSkill, UserSkillDto, UserSkillAddRequest, UserSkillUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _userSkillRepository = repository;
        _language = language;
    }

    public override async Task<UserSkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userSkill = await _userSkillRepository.GetWithSkillDataById(id, cancellationToken);
        
        return userSkill == null ? null : EntityToDtoMapper.MapUserSkillToDto(userSkill, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<UserSkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skills = await _userSkillRepository.GetAllActiveAsync(cancellationToken);

        return EntityToDtoMapper.MapUserSkillsToDtoList(skills, _language.LanguageCode);
    }

    public override async Task AddAsync(UserSkillAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newUserSkill = new UserSkill
        {
            Id = Guid.NewGuid(),
            SkillId = request.SkillId,
            Proficiency = request.Proficiency,
            CreatedAt = DateTime.UtcNow
        };
        
        await _userSkillRepository.AddAsync(newUserSkill, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);   
    }

    public override async Task UpdateAsync(UserSkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingUserSkill = await _userSkillRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingUserSkill is null) throw new Exception("User skill not found");
        
        existingUserSkill.Proficiency = request.Proficiency;
        
        await _userSkillRepository.UpdateAsync(existingUserSkill, cancellationToken);
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