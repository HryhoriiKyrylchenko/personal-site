using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Enums;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class LearningSkillRepository : EfRepository<LearningSkill>, ILearningSkillRepository
{
    public LearningSkillRepository(
        ApplicationDbContext context, 
        ILogger<LearningSkillRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<LearningSkill>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.LearningSkills
            .Where(ls => !ls.IsDeleted)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .Include(us => us.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .OrderBy(ls => ls.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<LearningSkill?> GetWithFullDataByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.LearningSkills
            .Include(ls => ls.Skill)
                .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .Include(ls => ls.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .FirstOrDefaultAsync(ls => ls.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsBySkillIdAsync(Guid skillId, CancellationToken cancellationToken)
    {
        if (skillId == Guid.Empty)
            throw new ArgumentException("SkillId cannot be empty", nameof(skillId));
        
        return await DbContext.LearningSkills
            .AnyAsync(ls => ls.SkillId == skillId, cancellationToken);   
    }

    public async Task<List<LearningSkill>> GetFilteredAsync(Guid? skillId, LearningStatus? status, CancellationToken cancellationToken = default)
    {
        var query = DbContext.LearningSkills.AsQueryable()
            .Include(x => x.Skill)
                .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .Include(x => x.Skill)
                .ThenInclude(s => s.Category)
                    .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
            .AsNoTracking();

        if (skillId.HasValue)
            query = query.Where(x => x.SkillId == skillId.Value);

        if (status.HasValue)
            query = query.Where(x => x.LearningStatus == status.Value);

        var entities = await query
            .OrderBy(s => s.Skill.Category.Key)
            .ToListAsync(cancellationToken);
        
        return entities;   
    }
}