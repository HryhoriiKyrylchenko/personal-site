namespace PersonalSite.Application.Services.Common;

public class ResumeService : 
    CrudServiceBase<Resume, ResumeDto, ResumeAddRequest, ResumeUpdateRequest>, 
    IResumeService
{
    IResumeRepository _resumeRepository;
    
    public ResumeService(
        IResumeRepository repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
        _resumeRepository = repository;
    }

    public override async Task<ResumeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapResumeToDto(entity);
    }

    public override async Task<IReadOnlyList<ResumeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        
        return EntityToDtoMapper.MapResumesToDtoList(entities);  
    }

    public override async Task AddAsync(ResumeAddRequest request, CancellationToken cancellationToken = default)
    {
        var newResume = new Resume
        {
            Id = Guid.NewGuid(),
            FileUrl = request.FileUrl,
            FileName = request.FileName,
            UploadedAt = DateTime.UtcNow,
            IsActive = true
        };
        
        await Repository.AddAsync(newResume, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(ResumeUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingResume = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingResume is null) throw new Exception("Resume not found");
        
        existingResume.FileUrl = request.FileUrl;
        existingResume.FileName = request.FileName;
        existingResume.IsActive = request.IsActive;
        
        Repository.Update(existingResume);
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

    public async Task<ResumeDto?> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        var resume = await _resumeRepository.GetLastActiveAsync(cancellationToken);
        
        return resume == null ? null : EntityToDtoMapper.MapResumeToDto(resume);
    }
}