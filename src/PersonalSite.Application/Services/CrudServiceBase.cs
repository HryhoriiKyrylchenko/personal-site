namespace PersonalSite.Application.Services;

public abstract class CrudServiceBase<TEntity, TDto, TAddRequest, TUpdateRequest> 
    : ICrudService<TDto, TAddRequest, TUpdateRequest>
    where TEntity : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IUnitOfWork UnitOfWork;

    protected CrudServiceBase(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork)
    {
        Repository = repository;
        UnitOfWork = unitOfWork;
    }

    public abstract Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public abstract Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
    public abstract Task AddAsync(TAddRequest request, CancellationToken cancellationToken = default);
    public abstract Task UpdateAsync(TUpdateRequest request, CancellationToken cancellationToken = default);
    public abstract Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
