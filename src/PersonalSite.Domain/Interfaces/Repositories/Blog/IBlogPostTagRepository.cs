using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Domain.Interfaces.Repositories.Blog;

public interface IBlogPostTagRepository : IRepository<BlogPostTag>
{
    Task<BlogPostTag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}