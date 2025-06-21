namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;

public class GetBlogPostByIdHandler : IRequestHandler<GetBlogPostByIdQuery, Result<BlogPostAdminDto>>
{
    private readonly IBlogPostRepository _repository;
    private readonly ILogger<GetBlogPostByIdHandler> _logger;

    public GetBlogPostByIdHandler(
        IBlogPostRepository repository,
        ILogger<GetBlogPostByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;   
    }
    
    public async Task<Result<BlogPostAdminDto>> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var blogPost = await _repository.GetByIdWithDataAsync(request.Id, cancellationToken);
            
            if (blogPost == null)
            {
                _logger.LogWarning("Blog post with ID {Id} not found.", request.Id);
                return Result<BlogPostAdminDto>.Failure("Blog post not found.");
            }
            
            var dto = BlogPostMapper.MapToAdminDto(blogPost);
            
            return Result<BlogPostAdminDto>.Success(dto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting blog post by id");
            return Result<BlogPostAdminDto>.Failure("Error occurred while getting blog post by id");      
        }
    }
}