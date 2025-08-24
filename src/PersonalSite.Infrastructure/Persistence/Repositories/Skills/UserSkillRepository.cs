using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class UserSkillRepository : EfRepository<UserSkill>, IUserSkillRepository
{
    public UserSkillRepository(
        ApplicationDbContext context, 
        ILogger<UserSkillRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<UserSkill>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.UserSkills
            .Where(us => !us.IsDeleted)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsBySkillIdAsync(Guid requestSkillId, CancellationToken cancellationToken)
    {
        if (requestSkillId == Guid.Empty)
            throw new ArgumentException("SkillId cannot be empty", nameof(requestSkillId));
        
        return await DbContext.UserSkills
            .AnyAsync(us => us.SkillId == requestSkillId, cancellationToken);   
    }

    public async Task<List<UserSkill>> GetFilteredAsync(Guid? skillId, short? minProficiency, short? maxProficiency,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.UserSkills.AsQueryable()
            .Include(x => x.Skill)
                .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .Include(x => x.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .AsSplitQuery()
            .AsNoTracking();

        if (skillId.HasValue)
            query = query.Where(x => x.SkillId == skillId.Value);

        if (minProficiency.HasValue)
            query = query.Where(x => x.Proficiency >= minProficiency.Value);

        if (maxProficiency.HasValue)
            query = query.Where(x => x.Proficiency <= maxProficiency.Value);

        var entities = await query
            .OrderBy(s => s.Skill.Category.Key)
            .ToListAsync(cancellationToken);
        
        return entities;   
    }
}