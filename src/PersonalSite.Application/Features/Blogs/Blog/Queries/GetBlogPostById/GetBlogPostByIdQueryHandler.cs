using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;

public class GetBlogPostByIdQueryHandler : IRequestHandler<GetBlogPostByIdQuery, Result<BlogPostAdminDto>>
{
    private readonly IBlogPostRepository _repository;
    private readonly ILogger<GetBlogPostByIdQueryHandler> _logger;
    private readonly IAdminMapper<BlogPost, BlogPostAdminDto> _mapper;

    public GetBlogPostByIdQueryHandler(
        IBlogPostRepository repository,
        ILogger<GetBlogPostByIdQueryHandler> logger,
        IAdminMapper<BlogPost, BlogPostAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;   
        _mapper = mapper;
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
            
            var dto = _mapper.MapToAdminDto(blogPost);
            
            return Result<BlogPostAdminDto>.Success(dto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting blog post by id");
            return Result<BlogPostAdminDto>.Failure("Error occurred while getting blog post by id");      
        }
    }
}