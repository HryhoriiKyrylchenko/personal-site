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
        var newLog = new LogEntry
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Level = request.Level,
            Message = request.Message,
            MessageTemplate = request.MessageTemplate,
            Exception = request.Exception,
            Properties = request.Properties,
            SourceContext = request.SourceContext
        };
        
        await Repository.AddAsync(newLog, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(LogEntryUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingLog = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingLog is null) throw new Exception("Log not found");
        
        existingLog.Level = request.Level;
        existingLog.Message = request.Message;
        existingLog.MessageTemplate = request.MessageTemplate;
        existingLog.Exception = request.Exception;
        existingLog.Properties = request.Properties;
        existingLog.SourceContext = request.SourceContext;
        
        await Repository.UpdateAsync(existingLog, cancellationToken);
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