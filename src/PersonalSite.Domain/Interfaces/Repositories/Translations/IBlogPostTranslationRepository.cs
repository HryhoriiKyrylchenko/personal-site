using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface IBlogPostTranslationRepository : IRepository<BlogPostTranslation>
{
    Task<List<BlogPostTranslation>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default);
}