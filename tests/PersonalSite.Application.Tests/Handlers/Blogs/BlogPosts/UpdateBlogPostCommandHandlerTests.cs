using PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Translations;
using PersonalSite.Infrastructure.Storage;

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
    private readonly Mock<IS3UrlBuilder> _urlBuilderMock = new();

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
            _loggerMock.Object,
            _urlBuilderMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ShouldUpdateBlogPost_WhenDataIsValid()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        var language = CommonTestDataFactory.CreateLanguage();
        var tagId = Guid.NewGuid();

        var blogPost = BlogPostTestDataFactory.CreateSimpleBlogPost(blogPostId);

        var translationDto = BlogPostTestDataFactory.CreateTranslationDto( 
            title: "Updated Title", 
            excerpt: "Updated Excerpt", 
            content: "Updated Content", 
            metaTitle: "Meta", 
            metaDescription: "MetaDesc", 
            ogImage: "og.jpg");

        var tagDto = BlogPostTestDataFactory.CreateTagDto(tagId);

        var request = new UpdateBlogPostCommand(
            Id: blogPostId,
            Slug: "new-slug",
            CoverImage: "new-cover.jpg",
            IsDeleted: true,
            Translations: [translationDto],
            Tags: [tagDto]
        );

        var existingTranslation = BlogPostTestDataFactory.CreateTranslation("en", blogPostId);
        existingTranslation.LanguageId = language.Id;
        existingTranslation.Language = language;

        var existingPostTag = BlogPostTestDataFactory.CreatePostTag(blogPostId);
    
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
    
        _tagRepoMock.Setup(r => r.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BlogPostTag { Id = tagId, Name = "Tech" });
    
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
        var request = BlogPostTestDataFactory.CreateValidUpdateCommand(Guid.NewGuid());

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
        var blogPost = BlogPostTestDataFactory.CreateBlogPost();

        var request = BlogPostTestDataFactory.CreateValidUpdateCommand(blogPost.Id);

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
        var blogPost = BlogPostTestDataFactory.CreateSimpleBlogPost(slug: "slug");
        var translation = BlogPostTestDataFactory.CreateTranslationDto("xx");
        var request = BlogPostTestDataFactory.CreateValidUpdateCommand(blogPost.Id, "slug", 
            translations: [translation]);

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
        //Arrange
        var blogPostId = Guid.NewGuid();

        var deTranslation = BlogPostTestDataFactory.CreateTranslation("de", blogPostId);
        var existingTag = BlogPostTestDataFactory.CreatePostTag(blogPostId);

        var blogPost = BlogPostTestDataFactory.CreateSimpleBlogPost(blogPostId);
        blogPost.Translations = [deTranslation];
        blogPost.PostTags = [existingTag];

        var enTranslationDto = BlogPostTestDataFactory.CreateTranslationDto();

        var request = BlogPostTestDataFactory.CreateValidUpdateCommand(
            id: blogPostId, slug: "slug", translations: [enTranslationDto]);

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(blogPost);

        _blogPostRepoMock.Setup(r => r.IsSlugAvailableAsync("slug", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _translationRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([deTranslation]);

        _postTagRepoMock.Setup(r => r.GetByBlogPostIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingTag]);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommonTestDataFactory.CreateLanguage());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _translationRepoMock.Verify(r => r.Remove(deTranslation), Times.Once);
        _postTagRepoMock.Verify(r => r.Remove(existingTag), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        var blogPost = BlogPostTestDataFactory.CreateSimpleBlogPost();
        
        var request = BlogPostTestDataFactory.CreateValidUpdateCommand(blogPost.Id);

        _blogPostRepoMock.Setup(r => r.GetByIdAsync(blogPost.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error updating blog post.");
    }
}