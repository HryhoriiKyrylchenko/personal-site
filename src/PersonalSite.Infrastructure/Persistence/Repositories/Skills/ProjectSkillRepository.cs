namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class ProjectSkillRepository : EfRepository<ProjectSkill>, IProjectSkillRepository
{
    public ProjectSkillRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ProjectSkill>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectSkills
            .Where(ps => ps.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProjectSkill>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectSkills
            .Where(ps => ps.SkillId == skillId)
            .ToListAsync(cancellationToken);
    }
}