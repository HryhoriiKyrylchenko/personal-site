using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;
using PersonalSite.Application.Tests.Fixtures;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts;

public class GetBlogPostByIdQueryHandlerTests
{
    private readonly Mock<IBlogPostRepository> _repositoryMock;
    private readonly Mock<IAdminMapper<BlogPost, BlogPostAdminDto>> _mapperMock;
    private readonly Mock<ILogger<GetBlogPostByIdQueryHandler>> _loggerMock;
    private readonly GetBlogPostByIdQueryHandler _handler;

    public GetBlogPostByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IBlogPostRepository>();
        _mapperMock = new Mock<IAdminMapper<BlogPost, BlogPostAdminDto>>();
        _loggerMock = new Mock<ILogger<GetBlogPostByIdQueryHandler>>();

        _handler = new GetBlogPostByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBlogPostFound()
    {
        // Arrange
        var blogPost = BlogPostTestDataFactory.CreateBlogPost();
        var dto = BlogPostTestDataFactory.MapToAdminDto(blogPost);

        _repositoryMock.Setup(r => r.GetByIdWithDataAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        _mapperMock.Setup(m => m.MapToAdminDto(blogPost)).Returns(dto);

        var query = new GetBlogPostByIdQuery(blogPost.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(dto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBlogPostNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdWithDataAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BlogPost?)null);

        var query = new GetBlogPostByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Blog post not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdWithDataAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        var query = new GetBlogPostByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error occurred while getting blog post by id");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error occurred while getting blog post by id")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once
        );
    }
}