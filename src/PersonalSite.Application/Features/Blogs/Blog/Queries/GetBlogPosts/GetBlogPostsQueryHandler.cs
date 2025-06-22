using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPosts;

public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, PaginatedResult<BlogPostAdminDto>>
{
    private readonly IBlogPostRepository _repository;
    private readonly ILogger<GetBlogPostsQueryHandler> _logger;
    private readonly IAdminMapper<BlogPost, BlogPostAdminDto> _mapper;   

    public GetBlogPostsQueryHandler(
        IBlogPostRepository repository,
        ILogger<GetBlogPostsQueryHandler> logger,
        IAdminMapper<BlogPost, BlogPostAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;   
        _mapper = mapper;   
    }

    public async Task<PaginatedResult<BlogPostAdminDto>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var blogPosts = await _repository.GetFilteredAsync(
                request.SlugFilter, 
                request.IsPublishedFilter,
                request.Page, 
                request.PageSize,
                cancellationToken);

            if (blogPosts.IsFailure || blogPosts.Value == null)
            {
                _logger.LogWarning("Blog posts not found");
                return PaginatedResult<BlogPostAdminDto>.Failure("Blog posts not found");
            }

            var items = _mapper.MapToAdminDtoList(blogPosts.Value);

            return PaginatedResult<BlogPostAdminDto>.Success(
                items, 
                blogPosts.PageNumber, 
                blogPosts.PageSize, 
                blogPosts.TotalCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting blog posts");
            return PaginatedResult<BlogPostAdminDto>.Failure("Error occurred while getting blog posts");       
        }
    }
}