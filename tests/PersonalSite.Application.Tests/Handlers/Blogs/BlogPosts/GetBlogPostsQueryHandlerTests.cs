using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPosts;
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
        var blogPosts = CreateTestBlogPosts();
        var expectedDtos = MapToExpectedDtos(blogPosts);

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

    private static List<BlogPost> CreateTestBlogPosts()
    {
        return
        [
            new BlogPost
            {
                Id = Guid.Parse("d3b27979-3d0b-4e68-932f-d2c7ec3229d9"),
                Slug = "post-1",
                CoverImage = "image_1.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsPublished = false,
                PublishedAt = null,
                IsDeleted = false,
                Translations = new List<BlogPostTranslation>
                {
                    new BlogPostTranslation
                    {
                        Id = Guid.Parse("7c37d2a7-f48b-4842-9e24-6e0369dd578d"),
                        LanguageId = Guid.Parse("f1a32e2c-45b4-4b3d-8c4f-326d91b51c4a"),
                        Language = new Language
                        {
                            Id = Guid.Parse("f1a32e2c-45b4-4b3d-8c4f-326d91b51c4a"),
                            Code = "en",
                            Name = "English"
                        },
                        BlogPostId = Guid.Parse("d3b27979-3d0b-4e68-932f-d2c7ec3229d9"),
                        Title = "Blog post 1 title",
                        Excerpt = "Blog post 1 Excerpt",
                        Content = "Blog post 1 Content",
                        MetaTitle = "Blog post 1 Meta Title",
                        MetaDescription = "Blog post 1 Meta Description",
                        OgImage = "image_1.jpg"
                    }
                },
                PostTags = new List<PostTag>
                {
                    new PostTag
                    {
                        Id = Guid.Parse("9e8d2a1b-7f6c-4d5e-b3a2-1c9f8b4d7e6a"),
                        BlogPostId = Guid.Parse("d3b27979-3d0b-4e68-932f-d2c7ec3229d9"),
                        BlogPostTagId = Guid.Parse("4f7e6d5c-3b2a-1d9c-8f7e-6d5c4b3a2d1e"),
                        BlogPostTag = new BlogPostTag
                        {
                            Id = Guid.Parse("4f7e6d5c-3b2a-1d9c-8f7e-6d5c4b3a2d1e"),
                            Name = "Tag 1"
                        }
                    }
                }
            },
            new BlogPost
            {
                Id = Guid.Parse("2a1b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                Slug = "post-2",
                CoverImage = "image_2.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                IsPublished = false,
                PublishedAt = null,
                IsDeleted = false,
                Translations = new List<BlogPostTranslation>
                {
                    new BlogPostTranslation
                    {
                        Id = Guid.Parse("8b7a6c5d-4e3f-2d1c-9b8a-7c6d5e4f3d2a"),
                        LanguageId = Guid.Parse("5c4d3e2f-1a9b-8c7d-6e5f-4d3c2b1a9e8d"),
                        Language = new Language
                        {
                            Id = Guid.Parse("5c4d3e2f-1a9b-8c7d-6e5f-4d3c2b1a9e8d"),
                            Code = "en",
                            Name = "English"
                        },
                        BlogPostId = Guid.Parse("2a1b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                        Title = "Blog post 2 title",
                        Excerpt = "Blog post 2 Excerpt",
                        Content = "Blog post 2 Content",
                        MetaTitle = "Blog post 2 Meta Title",
                        MetaDescription = "Blog post 2 Meta Description",
                        OgImage = "image_2.jpg"
                    }
                },
                PostTags = new List<PostTag>
                {
                    new PostTag
                    {
                        Id = Guid.Parse("3f2e1d9c-8b7a-6c5d-4e3f-2d1c9b8a7c6d"),
                        BlogPostId = Guid.Parse("2a1b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
                        BlogPostTagId = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-ef1234567890"),
                        BlogPostTag = new BlogPostTag
                        {
                            Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-ef1234567890"),
                            Name = "Tag 2"
                        }
                    }
                }
            }
        ];
    }

    private static List<BlogPostAdminDto> MapToExpectedDtos(List<BlogPost> blogPosts)
    {
        return blogPosts.Select(x => new BlogPostAdminDto
        {
            Id = x.Id,
            Slug = x.Slug,
            CoverImage = x.CoverImage,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            IsDeleted = x.IsDeleted,
            IsPublished = x.IsPublished,
            PublishedAt = x.PublishedAt,
            Translations = x.Translations.Select(MapToTranslationDto).ToList(),
            Tags = x.PostTags.Select(MapToTagDto).ToList()
        }).ToList();
    }

    private static BlogPostTranslationDto MapToTranslationDto(BlogPostTranslation t) =>
        new()
        {
            Id = t.Id,
            LanguageCode = t.Language.Code,
            BlogPostId = t.BlogPostId,
            Title = t.Title,
            Excerpt = t.Excerpt,
            Content = t.Content,
            MetaTitle = t.MetaTitle,
            MetaDescription = t.MetaDescription,
            OgImage = t.OgImage
        };

    private static BlogPostTagDto MapToTagDto(PostTag t) =>
        new()
        {
            Id = t.BlogPostTagId,
            Name = t.BlogPostTag.Name
        };

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