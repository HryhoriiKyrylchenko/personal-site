namespace PersonalSite.Application.Features.Blogs.Blog.Commands.DeleteBlogPost;

public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostCommand, Result>
{
    private readonly IBlogPostRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBlogPostHandler> _logger;

    public DeleteBlogPostHandler(
        IBlogPostRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteBlogPostHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var blogPost = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (blogPost == null)
                return Result.Failure("Blog post not found.");
        
            if (blogPost.IsDeleted)
                return Result.Failure("Blog post is already deleted.");

            blogPost.IsDeleted = true;
            blogPost.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting blog post.");
            return Result.Failure("Error deleting blog post.");
        }
    }
}