namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPosts;

public class GetBlogPostsHandler : IRequestHandler<GetBlogPostsQuery, PaginatedResult<BlogPostAdminDto>>
{
    private readonly IBlogPostRepository _repository;
    private readonly ILogger<GetBlogPostsHandler> _logger;

    public GetBlogPostsHandler(
        IBlogPostRepository repository,
        ILogger<GetBlogPostsHandler> logger)
    {
        _repository = repository;
        _logger = logger;   
    }

    public async Task<PaginatedResult<BlogPostAdminDto>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetQueryable()
                .Include(x => x.Translations)
                    .ThenInclude(t => t.Language)
                .Include(x => x.PostTags)
                    .ThenInclude(pt => pt.BlogPostTag)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.SlugFilter))
                query = query.Where(x => x.Slug.Contains(request.SlugFilter));

            if (request.IsPublishedFilter.HasValue)
                query = query.Where(x => x.IsPublished == request.IsPublishedFilter.Value);

            var total = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = EntityToDtoMapper.MapBlogPostsToAdminDtoList(entities);

            return PaginatedResult<BlogPostAdminDto>.Success(items, total, request.Page, request.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting blog posts");
            return PaginatedResult<BlogPostAdminDto>.Failure("Error occurred while getting blog posts");       
        }
    }
}