using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class ProjectSkillRepository : EfRepository<ProjectSkill>, IProjectSkillRepository
{
    public ProjectSkillRepository(
        ApplicationDbContext context, 
        ILogger<ProjectSkillRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<ProjectSkill>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        if (projectId == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(projectId));
        
        return await DbContext.ProjectSkills
            .Where(ps => ps.ProjectId == projectId)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Translations)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .ToListAsync(cancellationToken);
    }
}