using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Projects;

public class ProjectRepository : EfRepository<Project>, IProjectRepository
{
    public ProjectRepository(
        ApplicationDbContext context, 
        ILogger<ProjectRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<Project>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Projects
            .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                            .ThenInclude(t => t.Language)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetLastAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Projects
            .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                            .ThenInclude(t => t.Language)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Project?> GetWithFullDataAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.Projects
            .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                            .ThenInclude(t => t.Language)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> IsSlugAvailableAsync(string requestSlug, CancellationToken cancellationToken)
    {
        return await DbContext.Projects.AllAsync(p => p.Slug != requestSlug, cancellationToken);   
    }

    public async Task<PaginatedResult<Project>> GetFilteredAsync(int page, int pageSize, string? slugFilter, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Projects.AsQueryable()
            .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                            .ThenInclude(t => t.Language)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(slugFilter))
            query = query.Where(p => p.Slug.Contains(slugFilter));

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<Project>.Success(entities, page, pageSize, total);  
    }
}