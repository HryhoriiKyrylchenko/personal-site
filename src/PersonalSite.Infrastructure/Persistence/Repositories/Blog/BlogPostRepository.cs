using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class BlogPostRepository : EfRepository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(
        ApplicationDbContext context, 
        ILogger<BlogPostRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<BlogPost?> GetByIdWithDataAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.BlogPosts
            .AsNoTracking()
            .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Where(p => p.IsPublished && !p.IsDeleted)
            .Include(p => p.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .OrderByDescending(p => p.PublishedAt)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSlugAvailableAsync(string slug, CancellationToken cancellationToken)
    {
        return await DbContext.BlogPosts.AllAsync(p => p.Slug != slug, cancellationToken);   
    }

    public async Task<PaginatedResult<BlogPost>> GetFilteredAsync(string? slugFilter, bool? isPublished, int page, 
        int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbContext.BlogPosts
            .Include(x => x.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(x => x.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .AsQueryable()
            .AsSplitQuery()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(slugFilter))
            query = query.Where(x => x.Slug.Contains(slugFilter));

        if (isPublished.HasValue)
            query = query.Where(x => x.IsPublished == isPublished.Value);

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<BlogPost>.Success(entities, page, pageSize, total);
    }
}