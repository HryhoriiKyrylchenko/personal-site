namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface ISkillTranslationRepository : IRepository<SkillTranslation>
{
    Task<SkillTranslation?> GetBySkillIdAndLanguageAsync(Guid skillId, string languageCode, CancellationToken cancellationToken = default);
}