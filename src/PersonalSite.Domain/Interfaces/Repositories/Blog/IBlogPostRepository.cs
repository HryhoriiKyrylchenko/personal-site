using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Domain.Interfaces.Repositories.Blog;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<BlogPost?> GetByIdWithDataAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default);
    Task<bool> IsSlugAvailableAsync(string slug, CancellationToken cancellationToken);
    Task<PaginatedResult<BlogPost>> GetFilteredAsync(
        string? slugFilter,
        bool? isPublished,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}