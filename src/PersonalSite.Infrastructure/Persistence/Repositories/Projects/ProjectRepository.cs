namespace PersonalSite.Infrastructure.Persistence.Repositories.Projects;

public class ProjectRepository : EfRepository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Project?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbContext.Projects
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<List<Project>> GetAllWithTranslationsAsync(string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.Projects
            .Include(p => p.Translations.Where(t => t.LanguageCode == languageCode))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}