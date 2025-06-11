namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class LanguageRepository : EfRepository<Language>, ILanguageRepository
{
    public LanguageRepository(
        ApplicationDbContext context, 
        ILogger<LanguageRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await DbContext.Languages
            .FirstOrDefaultAsync(l => l.Code == code, cancellationToken);
    }
}