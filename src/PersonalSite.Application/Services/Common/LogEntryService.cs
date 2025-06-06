namespace PersonalSite.Application.Services.Common;

public class LogEntryService : 
    CrudServiceBase<LogEntry, LogEntryDto, LogEntryAddRequest, LogEntryUpdateRequest>, 
    ILogEntryService
{
    public LogEntryService(
        IRepository<LogEntry> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }
    
    public override async Task<LogEntryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapLogEntryToDto(entity);
    }

    public override async Task<IReadOnlyList<LogEntryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        return EntityToDtoMapper.MapLogEntriesToDtoList(entities);
    }

    public override async Task AddAsync(LogEntryAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(LogEntryUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}