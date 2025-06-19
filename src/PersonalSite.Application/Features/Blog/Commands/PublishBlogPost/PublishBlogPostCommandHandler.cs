namespace PersonalSite.Application.Features.Blog.Commands.PublishBlogPost;

public class PublishBlogPostCommandHandler : IRequestHandler<PublishBlogPostCommand, Result>
{
    private readonly IBlogPostRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundPublisher _publisher;
    private readonly ILogger<PublishBlogPostCommandHandler> _logger;

    public PublishBlogPostCommandHandler(
        IBlogPostRepository repository,
        IUnitOfWork unitOfWork,
        IBackgroundPublisher publisher,
        ILogger<PublishBlogPostCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Result> Handle(PublishBlogPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (post == null)
        {
            _logger.LogWarning("Blog post with ID {Id} not found.", request.Id);
            return Result.Failure("Blog post not found.");
        }

        post.UpdatedAt = DateTime.UtcNow;

        if (request.IsPublished)
        {
            if (request.PublishDate!.Value <= DateTime.UtcNow)
            {
                post.IsPublished = true;
                post.PublishedAt = DateTime.UtcNow;
            }
            else
            {
                _publisher.Schedule(
                    new PublishBlogPostCommand
                    {
                        Id = request.Id,
                        IsPublished = true,
                        PublishDate = request.PublishDate
                    },
                    request.PublishDate.Value
                );
            }
        }
        else
        {
            post.IsPublished = false;
            post.PublishedAt = null;
        }

        await _repository.UpdateAsync(post, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}