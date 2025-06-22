using PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts;

public class CreateBlogPostCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBlogPostRepository> _blogPostRepoMock = new();
    private readonly Mock<IBlogPostTranslationRepository> _translationRepoMock = new();
    private readonly Mock<IBlogPostTagRepository> _tagRepoMock = new();
    private readonly Mock<IPostTagRepository> _postTagRepoMock = new();
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<ILogger<CreateBlogPostCommandHandler>> _loggerMock = new();

    private readonly CreateBlogPostCommandHandler _handler;

    public CreateBlogPostCommandHandlerTests()
    {
        _handler = new CreateBlogPostCommandHandler(
            _unitOfWorkMock.Object,
            _blogPostRepoMock.Object,
            _translationRepoMock.Object,
            _tagRepoMock.Object,
            _postTagRepoMock.Object,
            _languageRepoMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateBlogPost_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateBlogPostCommand(
            Slug: "valid-slug",
            CoverImage: "cover.jpg",
            IsPublished: true,
            Translations:
            [
                new BlogPostTranslationDto
                {
                    Id = Guid.NewGuid(),
                    LanguageCode = "en",
                    Title = "Title",
                    Excerpt = "Excerpt",
                    Content = "Content",
                    MetaTitle = "MetaTitle",
                    MetaDescription = "MetaDesc",
                    OgImage = "og.jpg"
                }
            ],
            Tags:
            [
                new BlogPostTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Tech",
                }
            ]
        );

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("valid-slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Language { Id = Guid.NewGuid(), Code = "en", IsDeleted = false });

        _tagRepoMock.Setup(r => r.GetByNameAsync("Tech", It.IsAny<CancellationToken>()))
            .ReturnsAsync((BlogPostTag?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _blogPostRepoMock.Verify(r => r.AddAsync(It.IsAny<BlogPost>(), It.IsAny<CancellationToken>()), Times.Once);
        _translationRepoMock.Verify(r => r.AddAsync(It.IsAny<BlogPostTranslation>(), It.IsAny<CancellationToken>()), Times.Once);
        _tagRepoMock.Verify(r => r.AddAsync(It.IsAny<BlogPostTag>(), It.IsAny<CancellationToken>()), Times.Once);
        _postTagRepoMock.Verify(r => r.AddAsync(It.IsAny<PostTag>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenSlugAlreadyExists()
    {
        // Arrange
        var request = new CreateBlogPostCommand("existing-slug", "", false, [], []);

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("existing-slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Slug is already in use.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenLanguageNotFound()
    {
        // Arrange
        var request = new CreateBlogPostCommand(
            Slug: "valid-slug",
            CoverImage: "",
            IsPublished: false,
            Translations:
            [
                new BlogPostTranslationDto
                {
                    Id = Guid.NewGuid(),
                    LanguageCode = "unknown",
                    Title = "Title",
                    Excerpt = "Excerpt",
                    Content = "Content",
                    MetaTitle = "MetaTitle",
                    MetaDescription = "MetaDesc",
                    OgImage = "og.jpg"
                }
            ],
            Tags: []
        );

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("valid-slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("unknown", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Language unknown not found.");
    }

    [Fact]
    public async Task Handle_ShouldReuseExistingTag_WhenTagIdProvided()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var request = new CreateBlogPostCommand(
            Slug: "post-with-tag",
            CoverImage: "",
            IsPublished: true,
            Translations: [],
            Tags: [
                new BlogPostTagDto
                {
                    Id = tagId,
                    Name = "Tech"
                }
            ]
        );

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("post-with-tag", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _tagRepoMock.Setup(r => r.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BlogPostTag { Id = tagId, Name = "Tech" });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _tagRepoMock.Verify(r => r.AddAsync(It.IsAny<BlogPostTag>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var request = new CreateBlogPostCommand("slug", "", false, [], []);

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("slug", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error creating blog post.");
    }

}