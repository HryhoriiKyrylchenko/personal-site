using PersonalSite.Application.Features.Blogs.Blog.Commands.PublishBlogPost;
using PersonalSite.Application.Services.Common;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts;

public class PublishBlogPostCommandHandlerTests
{
    private readonly Mock<IBlogPostRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBackgroundPublisher> _publisherMock = new();
    private readonly Mock<ILogger<PublishBlogPostCommandHandler>> _loggerMock = new();
    private readonly PublishBlogPostCommandHandler _handler;

    public PublishBlogPostCommandHandlerTests()
    {
        _handler = new PublishBlogPostCommandHandler(
            _repoMock.Object,
            _unitOfWorkMock.Object,
            _publisherMock.Object,
            _loggerMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenBlogPostNotFound()
    {
        var request = new PublishBlogPostCommand { Id = Guid.NewGuid(), IsPublished = true, PublishDate = DateTime.UtcNow };

        _repoMock.Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BlogPost?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Blog post not found.");
    }

    [Fact]
    public async Task Handle_ShouldPublishImmediately_WhenDateIsNowOrPast()
    {
        var post = new BlogPost { Id = Guid.NewGuid(), IsPublished = false };
        var request = new PublishBlogPostCommand
        {
            Id = post.Id,
            IsPublished = true,
            PublishDate = DateTime.UtcNow.AddMinutes(-5)
        };

        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        post.IsPublished.Should().BeTrue();
        post.PublishedAt.Should().NotBeNull();
        _publisherMock.Verify(p => p.Schedule(It.IsAny<PublishBlogPostCommand>(), It.IsAny<DateTime>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldScheduleFuturePublish_WhenDateIsInFuture()
    {
        var post = new BlogPost { Id = Guid.NewGuid(), IsPublished = false };
        var futureDate = DateTime.UtcNow.AddHours(2);

        var request = new PublishBlogPostCommand
        {
            Id = post.Id,
            IsPublished = true,
            PublishDate = futureDate
        };

        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        post.IsPublished.Should().BeFalse(); 
        _publisherMock.Verify(p =>
            p.Schedule(It.Is<PublishBlogPostCommand>(cmd => cmd.Id == request.Id && cmd.IsPublished),
                futureDate), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUnpublishBlogPost_WhenIsPublishedIsFalse()
    {
        var post = new BlogPost { Id = Guid.NewGuid(), IsPublished = true, PublishedAt = DateTime.UtcNow };
        var request = new PublishBlogPostCommand { Id = post.Id, IsPublished = false };

        _repoMock.Setup(r => r.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        post.IsPublished.Should().BeFalse();
        post.PublishedAt.Should().BeNull();
        _publisherMock.Verify(p => p.Schedule(It.IsAny<PublishBlogPostCommand>(), It.IsAny<DateTime>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _handler.Handle(new PublishBlogPostCommand { Id = id, IsPublished = true, PublishDate = DateTime.UtcNow }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error publishing blog post."); 
    }
}
