using PersonalSite.Application.Features.Blogs.Blog.Commands.DeleteBlogPost;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts;

public class DeleteBlogPostCommandHandlerTests
{
    private readonly Mock<IBlogPostRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteBlogPostCommandHandler>> _loggerMock = new();
    private readonly DeleteBlogPostCommandHandler _handler;

    public DeleteBlogPostCommandHandlerTests()
    {
        _handler = new DeleteBlogPostCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldMarkBlogPostAsDeleted_WhenExistsAndNotDeleted()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        var blogPost = new BlogPost { Id = blogPostId, IsDeleted = false };

        _repositoryMock.Setup(r => r.GetByIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        // Act
        var result = await _handler.Handle(new DeleteBlogPostCommand(blogPostId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        blogPost.IsDeleted.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBlogPostNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BlogPost?)null);

        // Act
        var result = await _handler.Handle(new DeleteBlogPostCommand(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Blog post not found.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBlogPostAlreadyDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();
        var blogPost = new BlogPost { Id = id, IsDeleted = true };

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        // Act
        var result = await _handler.Handle(new DeleteBlogPostCommand(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Blog post is already deleted.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(new DeleteBlogPostCommand(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error deleting blog post.");
    }
}