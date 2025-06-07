namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class UserSkillRepository : EfRepository<UserSkill>, IUserSkillRepository
{
    public UserSkillRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<UserSkill>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken = default)
    {
        return await DbContext.UserSkills
            .Where(us => us.SkillId == skillId && !us.IsDeleted)
            .Include(us => us.Skill)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserSkill>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.UserSkills
            .Where(us => !us.IsDeleted)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Translations)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserSkill?> GetWithSkillDataById(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.UserSkills
            .Include(us => us.Skill)
                .ThenInclude(s => s.Translations)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations)
            .FirstOrDefaultAsync(us => us.Id == id, cancellationToken);   
    }
}