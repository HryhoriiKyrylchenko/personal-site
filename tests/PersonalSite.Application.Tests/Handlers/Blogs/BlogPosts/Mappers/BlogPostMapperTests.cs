using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Mappers;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts.Mappers;

public class BlogPostMapperTests
{
    private readonly Mock<IS3UrlBuilder> _urlBuilderMock;
    private readonly Mock<IMapper<BlogPostTag, BlogPostTagDto>> _tagMapperMock;
    private readonly Mock<IMapper<BlogPostTranslation, BlogPostTranslationDto>> _translationMapperMock;
    private readonly BlogPostMapper _mapper;

    public BlogPostMapperTests()
    {
        _urlBuilderMock = new Mock<IS3UrlBuilder>();
        _tagMapperMock = new Mock<IMapper<BlogPostTag, BlogPostTagDto>>();
        _translationMapperMock = new Mock<IMapper<BlogPostTranslation, BlogPostTranslationDto>>();
        _mapper = new BlogPostMapper(_urlBuilderMock.Object, _tagMapperMock.Object, _translationMapperMock.Object);
    }

    [Fact]
    public void MapToDto_Should_Map_Properties_And_Handle_Missing_Translation()
    {
        var coverImagePath = "cover.jpg";
        var ogImagePath = "og.jpg";
        var blogPost = new BlogPost
        {
            Id = Guid.NewGuid(),
            Slug = "slug",
            CoverImage = coverImagePath,
            IsPublished = true,
            PublishedAt = DateTime.UtcNow,
            Translations = new List<BlogPostTranslation>
            {
                new BlogPostTranslation
                {
                    Language = new Language { Code = "en" },
                    Title = "Title",
                    Excerpt = "Excerpt",
                    Content = "Content",
                    MetaTitle = "MetaTitle",
                    MetaDescription = "MetaDesc",
                    OgImage = ogImagePath
                }
            },
            PostTags = new List<PostTag>
            {
                new PostTag
                {
                    BlogPostTag = new BlogPostTag { Id = Guid.NewGuid(), Name = "Tag1" }
                }
            }
        };

        _urlBuilderMock.Setup(u => u.BuildUrl(coverImagePath)).Returns("url-cover");
        _urlBuilderMock.Setup(u => u.BuildUrl(ogImagePath)).Returns("url-og");
        _tagMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<BlogPostTag>>()))
            .Returns(new List<BlogPostTagDto> { new() { Id = Guid.NewGuid(), Name = "Tag1" } });

        var dto = _mapper.MapToDto(blogPost, "en");

        dto.Id.Should().Be(blogPost.Id);
        dto.Slug.Should().Be(blogPost.Slug);
        dto.CoverImage.Should().Be("url-cover");
        dto.IsPublished.Should().BeTrue();
        dto.Title.Should().Be("Title");
        dto.Excerpt.Should().Be("Excerpt");
        dto.Content.Should().Be("Content");
        dto.MetaTitle.Should().Be("MetaTitle");
        dto.MetaDescription.Should().Be("MetaDesc");
        dto.OgImage.Should().Be("url-og");
        dto.Tags.Should().NotBeEmpty();

        _urlBuilderMock.Verify(u => u.BuildUrl(coverImagePath), Times.Once);
        _urlBuilderMock.Verify(u => u.BuildUrl(ogImagePath), Times.Once);
        _tagMapperMock.Verify(m => m.MapToDtoList(It.IsAny<IEnumerable<BlogPostTag>>()), Times.Once);
    }

    [Fact]
    public void MapToDto_Should_Handle_Missing_Translation_Gracefully()
    {
        var blogPost = new BlogPost
        {
            Id = Guid.NewGuid(),
            Slug = "slug",
            CoverImage = "cover.jpg",
            Translations = new List<BlogPostTranslation>() // empty translations
        };

        _urlBuilderMock.Setup(u => u.BuildUrl(It.IsAny<string>())).Returns("url");

        _tagMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<BlogPostTag>>()))
            .Returns(new List<BlogPostTagDto>());

        var dto = _mapper.MapToDto(blogPost, "en");

        dto.Title.Should().BeEmpty();
        dto.Excerpt.Should().BeEmpty();
        dto.Content.Should().BeEmpty();
        dto.MetaTitle.Should().BeEmpty();
        dto.MetaDescription.Should().BeEmpty();
        dto.OgImage.Should().BeEmpty();
    }

    [Fact]
    public void MapToAdminDto_Should_Map_All_Properties_And_Call_Translation_And_Tag_Mappers()
    {
        var blogPost = new BlogPost
        {
            Id = Guid.NewGuid(),
            Slug = "slug",
            CoverImage = "cover.jpg",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false,
            IsPublished = true,
            PublishedAt = DateTime.UtcNow,
            Translations = new List<BlogPostTranslation>(),
            PostTags = new List<PostTag>()
        };

        _urlBuilderMock.Setup(u => u.BuildUrl(It.IsAny<string>())).Returns("url");
        _translationMapperMock.Setup(m => m.MapToDtoList(blogPost.Translations)).Returns(new List<BlogPostTranslationDto>());
        _tagMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<BlogPostTag>>())).Returns(new List<BlogPostTagDto>());

        var adminDto = _mapper.MapToAdminDto(blogPost);

        adminDto.Id.Should().Be(blogPost.Id);
        adminDto.Slug.Should().Be(blogPost.Slug);
        adminDto.CoverImage.Should().Be("url");
        adminDto.Translations.Should().NotBeNull();
        adminDto.Tags.Should().NotBeNull();

        _translationMapperMock.Verify(m => m.MapToDtoList(blogPost.Translations), Times.Once);
        _tagMapperMock.Verify(m => m.MapToDtoList(It.IsAny<IEnumerable<BlogPostTag>>()), Times.Once);
    }
}