namespace PersonalSite.Application.Services;

public interface ICrudService<TDto, in TAddRequest, in TUpdateRequest> : IReadOnlyService<TDto>
{
    Task AddAsync(TAddRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(TUpdateRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}