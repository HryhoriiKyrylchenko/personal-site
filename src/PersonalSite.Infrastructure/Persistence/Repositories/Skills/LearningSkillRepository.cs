namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class LearningSkillRepository : EfRepository<LearningSkill>, ILearningSkillRepository
{
    public LearningSkillRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<LearningSkill>> GetByStatusAsync(LearningStatus status, CancellationToken cancellationToken = default)
    {
        return await DbContext.LearningSkills
            .Where(ls => ls.LearningStatus == status && !ls.IsDeleted)
            .OrderBy(ls => ls.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LearningSkill>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.LearningSkills
            .Where(ls => !ls.IsDeleted)
            .OrderBy(ls => ls.DisplayOrder)
            .ToListAsync(cancellationToken);
    }
}