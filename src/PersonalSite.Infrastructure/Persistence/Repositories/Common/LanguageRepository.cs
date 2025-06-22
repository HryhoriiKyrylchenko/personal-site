using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LanguageRepository : EfRepository<Language>, ILanguageRepository
{
    public LanguageRepository(
        ApplicationDbContext context, 
        ILogger<LanguageRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or whitespace", nameof(code));
        
        return await DbContext.Languages.AnyAsync(l => l.Code == code, cancellationToken);   
    }

    public async Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be null or whitespace", nameof(code));
        
        return await DbContext.Languages.FirstOrDefaultAsync(l => l.Code == code, cancellationToken);  
    }
}