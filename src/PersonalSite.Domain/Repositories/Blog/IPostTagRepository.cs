namespace PersonalSite.Domain.Repositories.Blog;

public interface IPostTagRepository : IRepository<PostTag>
{
    Task<List<PostTag>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default);
    Task<PostTag?> GetByBlogPostAndTagAsync(Guid blogPostId, Guid blogPostTagId, CancellationToken cancellationToken = default);
}