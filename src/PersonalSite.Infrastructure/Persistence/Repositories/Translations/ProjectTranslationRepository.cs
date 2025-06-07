namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class ProjectTranslationRepository : EfRepository<ProjectTranslation>, IProjectTranslationRepository
{
    public ProjectTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ProjectTranslation?> GetByProjectIdAndLanguageAsync(Guid projectId, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectTranslations
            .FirstOrDefaultAsync(pt => pt.ProjectId == projectId && pt.Language.Code == languageCode, cancellationToken);
    }
}