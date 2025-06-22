using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface ISkillTranslationRepository : IRepository<SkillTranslation>
{
    Task<List<SkillTranslation>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken);
}