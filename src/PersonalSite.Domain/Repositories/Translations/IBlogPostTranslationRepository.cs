namespace PersonalSite.Domain.Repositories.Translations;

public interface IBlogPostTranslationRepository : IRepository<BlogPostTranslation>
{
    Task<List<BlogPostTranslation>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default);
    Task<BlogPostTranslation?> GetByBlogPostIdAndLanguageAsync(Guid blogPostId, string languageCode, CancellationToken cancellationToken = default);
}