namespace PersonalSite.Domain.Repositories.Blog;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default);
}