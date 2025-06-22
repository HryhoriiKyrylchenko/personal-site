using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Domain.Interfaces.Repositories.Blog;

public interface IPostTagRepository : IRepository<PostTag>
{
    Task<List<PostTag>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default);
}