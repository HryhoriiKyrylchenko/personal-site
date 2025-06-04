namespace PersonalSite.Domain.Repositories.Blog;

public interface IBlogPostTagRepository : IRepository<BlogPostTag>
{
    Task<BlogPostTag?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<BlogPostTag>> GetAllSortedAsync(CancellationToken cancellationToken = default);
}