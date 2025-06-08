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

    public async Task<List<Project>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Projects
            .Include(p => p.Translations)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetLastAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Projects
            .Include(p => p.Translations)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Project?> GetWithFullDataAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Projects
            .Include(p => p.Translations)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}