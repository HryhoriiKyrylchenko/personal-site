using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPosts;
using PersonalSite.Application.Tests.Fixtures;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts;

public class GetBlogPostsQueryHandlerTests
{
    private readonly Mock<IBlogPostRepository> _repositoryMock;
    private readonly Mock<ILogger<GetBlogPostsQueryHandler>> _loggerMock;
    private readonly Mock<IAdminMapper<BlogPost, BlogPostAdminDto>> _mapperMock;
    private readonly GetBlogPostsQueryHandler _handler;

    public GetBlogPostsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IBlogPostRepository>();
        _loggerMock = new Mock<ILogger<GetBlogPostsQueryHandler>>();
        _mapperMock = new Mock<IAdminMapper<BlogPost, BlogPostAdminDto>>();

        _handler = new GetBlogPostsQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBlogPostsExist()
    {
        // Arrange
        var blogPosts = new List<BlogPost>
        {
            BlogPostTestDataFactory.CreateBlogPost(slug: "Post-1"),
            BlogPostTestDataFactory.CreateBlogPost(slug: "Post-2")
        };
        var expectedDtos = blogPosts.Select(BlogPostTestDataFactory.MapToAdminDto).ToList();

        var paginatedResult = PaginatedResult<BlogPost>.Success(
            blogPosts, pageNumber: 1, pageSize: 10, totalCount: blogPosts.Count);

        _repositoryMock
            .Setup(r => r.GetFilteredAsync(null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        _mapperMock
            .Setup(m => m.MapToAdminDtoList(blogPosts))
            .Returns(expectedDtos);

        var query = new GetBlogPostsQuery(Page: 1, PageSize: 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedDtos);
        result.TotalCount.Should().Be(blogPosts.Count);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryReturnsFailure()
    {
        // Arrange
        var failureResult = PaginatedResult<BlogPost>.Failure("Error fetching data");

        _repositoryMock
            .Setup(r => r.GetFilteredAsync(null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(failureResult);

        var query = new GetBlogPostsQuery(Page: 1, PageSize: 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Blog posts not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetFilteredAsync(null, null, 1, 10, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB Error"));

        var query = new GetBlogPostsQuery(Page: 1, PageSize: 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error occurred while getting blog posts");
    }
}