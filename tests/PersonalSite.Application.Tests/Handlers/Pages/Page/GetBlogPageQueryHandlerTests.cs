using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetBlogPageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly Mock<ITranslatableMapper<BlogPost, BlogPostDto>> _blogPostMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetBlogPageQueryHandler _handler;

    public GetBlogPageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
        var loggerMock = new Mock<ILogger<GetBlogPageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();
        _blogPostMapperMock = new Mock<ITranslatableMapper<BlogPost, BlogPostDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetBlogPageQueryHandler(
            _languageContext,
            _pageRepositoryMock.Object,
            _blogPostRepositoryMock.Object,
            loggerMock.Object,
            _pageMapperMock.Object,
            _blogPostMapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_InvalidLanguageContext_ReturnsFailure()
    {
        // Arrange
        _languageContext.LanguageCode = null!;
        var query = new GetBlogPageQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid language context.");
    }

    [Fact]
    public async Task Handle_BlogPageNotFound_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("blog", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(new GetBlogPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Blog page not found.");
    }

    [Fact]
    public async Task Handle_NoBlogPostsFound_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto { Title = "Blog Page" };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("blog", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);
        _pageMapperMock.Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        _blogPostRepositoryMock.Setup(r => r.GetPublishedPostsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<BlogPost>());

        _blogPostMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<BlogPost>>(), "en"))
            .Returns(new List<BlogPostDto>());

        // Act
        var result = await _handler.Handle(new GetBlogPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.BlogPosts.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_BlogPageAndPostsFound_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto { Title = "Blog Page" };

        var blogPost = new BlogPost { Id = Guid.NewGuid() };
        var blogPostDto = new BlogPostDto { Id = blogPost.Id };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("blog", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);
        _pageMapperMock.Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        _blogPostRepositoryMock.Setup(r => r.GetPublishedPostsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<BlogPost> { blogPost });

        _blogPostMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<BlogPost>>(), "en"))
            .Returns([blogPostDto]);

        // Act
        var result = await _handler.Handle(new GetBlogPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.BlogPosts.Should().ContainSingle();
        result.Value.BlogPosts[0].Id.Should().Be(blogPost.Id);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailureAndLogsError()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(new GetBlogPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}