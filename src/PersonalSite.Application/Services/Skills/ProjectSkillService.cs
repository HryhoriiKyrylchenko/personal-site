namespace PersonalSite.Application.Services.Skills;

public class ProjectSkillService : 
    CrudServiceBase<ProjectSkill, ProjectSkillDto, ProjectSkillAddRequest, ProjectSkillUpdateRequest>, 
    IProjectSkillService
{
    private readonly LanguageContext _language;
    private readonly IProjectSkillRepository _projectSkillRepository;
    
    public ProjectSkillService(
        IProjectSkillRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language) 
        : base(repository, unitOfWork)
    {
        _projectSkillRepository = repository;
        _language = language;
    }

    public override async Task<ProjectSkillDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var projectSkill = await _projectSkillRepository.GetWithSkillDataById(id, cancellationToken); 
        
        return projectSkill == null ? null : EntityToDtoMapper.MapProjectSkillToDto(projectSkill, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<ProjectSkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projectSkills = await _projectSkillRepository.GetAllWithSkillData(cancellationToken);
        
        return EntityToDtoMapper.MapProjectSkillsToDtoList(projectSkills, _language.LanguageCode);
    }

    public override async Task AddAsync(ProjectSkillAddRequest request, CancellationToken cancellationToken = default)
    {
        var newProjectSkill = new ProjectSkill
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            SkillId = request.SkillId
        };
        
        await _projectSkillRepository.AddAsync(newProjectSkill, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(ProjectSkillUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingProjectSkill = await _projectSkillRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingProjectSkill is null) throw new Exception("Project skill not found");
        
        existingProjectSkill.SkillId = request.SkillId;
        
        _projectSkillRepository.Update(existingProjectSkill);
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

    public async Task<IReadOnlyList<ProjectSkillDto>> GetAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var projectSkills = await _projectSkillRepository.GetByProjectIdAsync(projectId, cancellationToken);
        
        return EntityToDtoMapper.MapProjectSkillsToDtoList(projectSkills, _language.LanguageCode);
    }
}