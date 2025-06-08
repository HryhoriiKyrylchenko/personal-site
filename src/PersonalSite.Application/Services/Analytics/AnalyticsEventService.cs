namespace PersonalSite.Application.Services.Analytics;

public class AnalyticsEventService : 
    CrudServiceBase<AnalyticsEvent, AnalyticsEventDto, AnalyticsEventAddRequest, AnalyticsEventUpdateRequest>,
    IAnalyticsEventService
{
    public AnalyticsEventService(
        IRepository<AnalyticsEvent> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }
    
    public override async Task<AnalyticsEventDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapAnalyticsEventToDto(entity);
    }

    public override async Task<IReadOnlyList<AnalyticsEventDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        return EntityToDtoMapper.MapAnalyticsEventsToDtoList(entities);
    }

    public override async Task AddAsync(AnalyticsEventAddRequest request, CancellationToken cancellationToken = default)
    {
        var newEvent = new AnalyticsEvent
        {
            Id = Guid.NewGuid(),
            EventType = request.EventType,
            PageSlug = request.PageSlug,
            Referrer = request.Referrer,
            UserAgent = request.UserAgent,
            CreatedAt = DateTime.UtcNow,
            AdditionalDataJson = request.AdditionalDataJson
        };
        
        await Repository.AddAsync(newEvent, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(AnalyticsEventUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingEvent = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingEvent is null) throw new Exception("Event not found");
        
        existingEvent.PageSlug = request.PageSlug;
        existingEvent.Referrer = request.Referrer;
        existingEvent.UserAgent = request.UserAgent;
        existingEvent.AdditionalDataJson = request.AdditionalDataJson;
        
        Repository.Update(existingEvent);
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