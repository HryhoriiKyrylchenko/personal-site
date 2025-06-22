using PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts;

public class UpdateBlogPostCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBlogPostRepository> _blogPostRepoMock = new();
    private readonly Mock<IBlogPostTranslationRepository> _translationRepoMock = new();
    private readonly Mock<IBlogPostTagRepository> _tagRepoMock = new();
    private readonly Mock<IPostTagRepository> _postTagRepoMock = new();
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<ILogger<UpdateBlogPostCommandHandler>> _loggerMock = new();

    private readonly UpdateBlogPostCommandHandler _handler;

    public UpdateBlogPostCommandHandlerTests()
    {
        _handler = new UpdateBlogPostCommandHandler(
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
    public async Task Handle_ShouldUpdateBlogPost_WhenDataIsValid()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        var languageId = Guid.NewGuid();
        var existingTagId = Guid.NewGuid();

        var blogPost = new BlogPost
        {
            Id = blogPostId,
            Slug = "old-slug",
            CoverImage = "old-cover.jpg",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        var language = new Language { Id = languageId, Code = "en" };

        var translationDto = new BlogPostTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = "en",
            Title = "Updated Title",
            Excerpt = "Updated Excerpt",
            Content = "Updated Content",
            MetaTitle = "Meta",
            MetaDescription = "MetaDesc",
            OgImage = "og.jpg"
        };

        var tagDto = new BlogPostTagDto
        {
            Id = existingTagId,
            Name = "Tech"
        };

        var request = new UpdateBlogPostCommand(
            Id: blogPostId,
            Slug: "new-slug",
            CoverImage: "new-cover.jpg",
            IsDeleted: true,
            Translations: [translationDto],
            Tags: [tagDto]
        );

        var existingTranslation = new BlogPostTranslation
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            LanguageId = languageId,
            Language = language
        };

        var existingPostTag = new PostTag
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            BlogPostTagId = Guid.NewGuid()
        };

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("new-slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);

        _translationRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingTranslation]);

        _postTagRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingPostTag]);

        _tagRepoMock.Setup(r => r.GetByIdAsync(existingTagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BlogPostTag { Id = existingTagId, Name = "Tech" });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _blogPostRepoMock.Verify(r => r.UpdateAsync(It.IsAny<BlogPost>(), It.IsAny<CancellationToken>()), Times.Once);
        _translationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<BlogPostTranslation>(), It.IsAny<CancellationToken>()), Times.Once);
        _postTagRepoMock.Verify(r => r.AddAsync(It.IsAny<PostTag>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenBlogPostDoesNotExist()
    {
        // Arrange
        var request = new UpdateBlogPostCommand(
            Id: Guid.NewGuid(),
            Slug: "slug",
            CoverImage: "cover.jpg",
            IsDeleted: false,
            Translations: [],
            Tags: []
        );

        _blogPostRepoMock
            .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BlogPost?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Blog post not found.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenSlugIsNotAvailable()
    {
        // Arrange
        var blogPost = new BlogPost
        {
            Id = Guid.NewGuid(),
            Slug = "original-slug"
        };

        var request = new UpdateBlogPostCommand(
            Id: blogPost.Id,
            Slug: "new-slug", // different slug
            CoverImage: "image.jpg",
            IsDeleted: false,
            Translations: [],
            Tags: []
        );

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("new-slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("A skill with this key already exists.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenLanguageDoesNotExist()
    {
        // Arrange
        var blogPost = new BlogPost
        {
            Id = Guid.NewGuid(),
            Slug = "slug"
        };

        var translation = new BlogPostTranslationDto
        {
            LanguageCode = "xx", // not found
            Title = "T", Excerpt = "E", Content = "C",
            MetaTitle = "M", MetaDescription = "MD", OgImage = "og.jpg"
        };

        var request = new UpdateBlogPostCommand(
            Id: blogPost.Id,
            Slug: "slug",
            CoverImage: "img.jpg",
            IsDeleted: false,
            Translations: [translation],
            Tags: []
        );

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("xx", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        _translationRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Language xx not found.");
    }

    [Fact]
    public async Task Handle_ShouldRemoveMissingTranslationsAndTags()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        var languageId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        var blogPost = new BlogPost { Id = blogPostId, Slug = "slug" };

        var existingTranslation = new BlogPostTranslation
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            LanguageId = languageId,
            Language = new Language { Id = languageId, Code = "de" }
        };

        var existingTag = new PostTag
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            BlogPostTagId = tagId
        };

        var translationDto = new BlogPostTranslationDto
        {
            LanguageCode = "en", Title = "", Excerpt = "", Content = "", MetaTitle = "", MetaDescription = "", OgImage = ""
        };

        var request = new UpdateBlogPostCommand(
            Id: blogPostId,
            Slug: "slug",
            CoverImage: "img",
            IsDeleted: false,
            Translations: [translationDto], // Only "en", so "de" should be removed
            Tags: [] // Tag should be removed
        );

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _translationRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingTranslation]);

        _postTagRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPostId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingTag]);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Language { Id = Guid.NewGuid(), Code = "en" });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _translationRepoMock.Verify(r => r.Remove(existingTranslation), Times.Once);
        _postTagRepoMock.Verify(r => r.Remove(existingTag), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var blogPost = new BlogPost { Id = Guid.NewGuid(), Slug = "slug" };

        var request = new UpdateBlogPostCommand(
            blogPost.Id,
            "slug", "img.jpg", false, [], []
        );

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error updating blog post.");
    }

}