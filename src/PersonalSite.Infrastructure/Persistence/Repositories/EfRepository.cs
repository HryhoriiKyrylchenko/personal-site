namespace PersonalSite.Infrastructure.Persistence.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly ILogger<EfRepository<T>> Logger;
    private readonly IValidator<T>? _validator;

    public EfRepository(
        ApplicationDbContext dbContext, 
        ILogger<EfRepository<T>> logger,
        IServiceProvider serviceProvider)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = serviceProvider.GetService<IValidator<T>>();
    }
    
    private async Task ValidateAsync(T entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (_validator is null)
            return;

        var result = await _validator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            var entityName = typeof(T).Name;
            var errorDetails = string.Join("; ", 
                result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

            Logger.LogWarning("Validation failed for entity {Entity}: {Errors}", entityName, errorDetails);

            throw new ValidationException(result.Errors);
        }
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbContext.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await ValidateAsync(entity, cancellationToken);
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var enumerable = entities.ToList();
        
        foreach (var entity in enumerable)
            await ValidateAsync(entity, cancellationToken);
        
        await DbContext.Set<T>().AddRangeAsync(enumerable, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        await ValidateAsync(entity, cancellationToken);
        DbContext.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        DbContext.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        DbContext.Set<T>().RemoveRange(entities);
    }
}