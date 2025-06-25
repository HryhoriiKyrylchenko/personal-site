using PersonalSite.Application.Features.Blogs.Blog.Mappers;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Blogs.BlogPosts;

public class BlogPostTranslationMapperTests
{
    private readonly Mock<IS3UrlBuilder> _urlBuilderMock;
    private readonly BlogPostTranslationMapper _mapper;

    public BlogPostTranslationMapperTests()
    {
        _urlBuilderMock = new Mock<IS3UrlBuilder>();
        _mapper = new BlogPostTranslationMapper(_urlBuilderMock.Object);
    }

    [Fact]
    public void MapToDto_Should_Map_All_Properties_And_Use_UrlBuilder()
    {
        var entity = new BlogPostTranslation
        {
            Id = Guid.NewGuid(),
            Language = new Language { Code = "en" },
            BlogPostId = Guid.NewGuid(),
            Title = "Title",
            Excerpt = "Excerpt",
            Content = "Content",
            MetaTitle = "Meta",
            MetaDescription = "MetaDesc",
            OgImage = "image.jpg"
        };

        _urlBuilderMock.Setup(x => x.BuildUrl("image.jpg")).Returns("https://s3.amazonaws.com/image.jpg");

        var dto = _mapper.MapToDto(entity);

        dto.Id.Should().Be(entity.Id);
        dto.LanguageCode.Should().Be("en");
        dto.BlogPostId.Should().Be(entity.BlogPostId);
        dto.Title.Should().Be(entity.Title);
        dto.Excerpt.Should().Be(entity.Excerpt);
        dto.Content.Should().Be(entity.Content);
        dto.MetaTitle.Should().Be(entity.MetaTitle);
        dto.MetaDescription.Should().Be(entity.MetaDescription);
        dto.OgImage.Should().Be("https://s3.amazonaws.com/image.jpg");

        _urlBuilderMock.Verify(x => x.BuildUrl("image.jpg"), Times.Once);
    }

    [Fact]
    public void MapToDto_Should_Return_EmptyString_For_Empty_OgImage()
    {
        var entity = new BlogPostTranslation
        {
            OgImage = "   ",
            Language = new Language { Code = "en" }
        };

        var dto = _mapper.MapToDto(entity);

        dto.OgImage.Should().BeEmpty();
        _urlBuilderMock.Verify(x => x.BuildUrl(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void MapToDtoList_Should_Map_All_Entities()
    {
        var entities = new List<BlogPostTranslation>
        {
            new BlogPostTranslation { Id = Guid.NewGuid(), Language = new Language { Code = "en" }, OgImage = "img1.jpg" },
            new BlogPostTranslation { Id = Guid.NewGuid(), Language = new Language { Code = "fr" }, OgImage = "" }
        };

        _urlBuilderMock.Setup(x => x.BuildUrl("img1.jpg")).Returns("url1");

        var dtos = _mapper.MapToDtoList(entities);

        dtos.Should().HaveCount(2);
        dtos[0].LanguageCode.Should().Be("en");
        dtos[0].OgImage.Should().Be("url1");
        dtos[1].LanguageCode.Should().Be("fr");
        dtos[1].OgImage.Should().BeEmpty();
    }
}
