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
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Translations)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProjectSkill>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectSkills
            .Where(ps => ps.SkillId == skillId)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Translations)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectSkill?> GetWithSkillDataById(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectSkills
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Translations)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);
    }

    public async Task<List<ProjectSkill>> GetAllWithSkillData(CancellationToken cancellationToken = default)
    {
        return await DbContext.ProjectSkills
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Translations)
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .ToListAsync(cancellationToken);
    }
}