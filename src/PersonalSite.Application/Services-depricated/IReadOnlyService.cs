namespace PersonalSite.Application.Services;

public interface IReadOnlyService<TDto>
{
    Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
}