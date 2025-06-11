namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class ProjectTranslationRepository : EfRepository<ProjectTranslation>, IProjectTranslationRepository
{
    public ProjectTranslationRepository(
        ApplicationDbContext context, 
        ILogger<ProjectTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<ProjectTranslation?> GetByProjectIdAndLanguageAsync(Guid projectId, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectTranslations
            .FirstOrDefaultAsync(pt => pt.ProjectId == projectId && pt.Language.Code == languageCode, cancellationToken);
    }

    public async Task<ProjectTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.ProjectTranslations
            .Include(t => t.Language)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ProjectTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken)
    {
        return await DbContext.ProjectTranslations
            .Include(t => t.Language)
            .ToListAsync(cancellationToken);   
    }
}