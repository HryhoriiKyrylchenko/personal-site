namespace PersonalSite.Domain.Interfaces.Repositories.Blog;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetByIdWithTagsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BlogPost>> GetAllWithTagsAsync(CancellationToken cancellationToken);
}