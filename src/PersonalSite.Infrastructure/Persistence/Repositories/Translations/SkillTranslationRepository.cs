using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillTranslationRepository : EfRepository<SkillTranslation>, ISkillTranslationRepository
{
    public SkillTranslationRepository(
        ApplicationDbContext context, 
        ILogger<SkillTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }


    public async Task<List<SkillTranslation>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken)
    {
        if (skillId == Guid.Empty)
            throw new ArgumentException("Skill ID cannot be empty", nameof(skillId));
        
        return await DbContext.SkillTranslations
            .Where(t => t.SkillId == skillId)
            .Include(t => t.Language)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}