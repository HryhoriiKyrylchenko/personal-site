namespace PersonalSite.Application.Services;

public abstract class CrudServiceBase<TEntity, TDto, TAddRequest, TUpdateRequest> 
    : ICrudService<TDto, TAddRequest, TUpdateRequest>
    where TEntity : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger<CrudServiceBase<TEntity, TDto, TAddRequest, TUpdateRequest>> Logger;
    private readonly IValidator<TAddRequest>? _addRequestValidator;
    private readonly IValidator<TUpdateRequest>? _updateRequestValidator;

    protected CrudServiceBase(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        ILogger<CrudServiceBase<TEntity, TDto, TAddRequest, TUpdateRequest>> logger,
        IServiceProvider serviceProvider)
    {
        Repository = repository;
        UnitOfWork = unitOfWork;
        Logger = logger;
        _addRequestValidator = serviceProvider.GetService<IValidator<TAddRequest>>();
        _updateRequestValidator = serviceProvider.GetService<IValidator<TUpdateRequest>>();
    }
    
    protected async Task ValidateAddRequestAsync(TAddRequest entity, CancellationToken cancellationToken)
    {
        if (_addRequestValidator is null)
            return;
        
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
    
        var result = await _addRequestValidator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            var entityName = typeof(TEntity).Name;
            var errorDetails = string.Join("; ", 
                result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
    
            Logger.LogWarning("Validation failed for entity {Entity}: {Errors}", entityName, errorDetails);
    
            throw new ValidationException(result.Errors);
        }
    }
    
    protected async Task ValidateUpdateRequestAsync(TUpdateRequest entity, CancellationToken cancellationToken)
    {
        if (_updateRequestValidator is null)
            return;
        
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
    
        var result = await _updateRequestValidator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            var entityName = typeof(TEntity).Name;
            var errorDetails = string.Join("; ", 
                result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
    
            Logger.LogWarning("Validation failed for entity {Entity}: {Errors}", entityName, errorDetails);
    
            throw new ValidationException(result.Errors);
        }
    }

    public abstract Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public abstract Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
    public abstract Task AddAsync(TAddRequest request, CancellationToken cancellationToken = default);
    public abstract Task UpdateAsync(TUpdateRequest request, CancellationToken cancellationToken = default);
    public abstract Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
