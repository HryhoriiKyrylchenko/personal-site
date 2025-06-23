using PersonalSite.Application.Features.Pages.Page.Mappers;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Pages.Page;

public class PageTranslationMapperTests
{
    private readonly Mock<IS3UrlBuilder> _urlBuilderMock;
    private readonly PageTranslationMapper _mapper;

    public PageTranslationMapperTests()
    {
        _urlBuilderMock = new Mock<IS3UrlBuilder>();
        _mapper = new PageTranslationMapper(_urlBuilderMock.Object);
    }

    [Fact]
    public void MapToDto_WithValidOgImage_ReturnsMappedDtoWithUrl()
    {
        // Arrange
        var ogImageKey = "image.png";
        var ogImageUrl = "https://cdn.example.com/image.png";
        var entity = new PageTranslation
        {
            Id = Guid.NewGuid(),
            Language = new Language { Code = "en" },
            PageId = Guid.NewGuid(),
            Data = new Dictionary<string, string> { { "foo", "bar" } },
            Title = "Title",
            Description = "Description",
            MetaTitle = "Meta Title",
            MetaDescription = "Meta Description",
            OgImage = ogImageKey
        };

        _urlBuilderMock.Setup(u => u.BuildUrl(ogImageKey)).Returns(ogImageUrl);

        // Act
        var dto = _mapper.MapToDto(entity);

        // Assert
        dto.Id.Should().Be(entity.Id);
        dto.LanguageCode.Should().Be(entity.Language.Code);
        dto.PageId.Should().Be(entity.PageId);
        dto.Data.Should().BeEquivalentTo(entity.Data);
        dto.Title.Should().Be(entity.Title);
        dto.Description.Should().Be(entity.Description);
        dto.MetaTitle.Should().Be(entity.MetaTitle);
        dto.MetaDescription.Should().Be(entity.MetaDescription);
        dto.OgImage.Should().Be(ogImageUrl);

        _urlBuilderMock.Verify(u => u.BuildUrl(ogImageKey), Times.Once);
    }

    [Fact]
    public void MapToDto_WithNullOrWhitespaceOgImage_ReturnsEmptyOgImage()
    {
        // Arrange
        var entityWithNullOgImage = new PageTranslation
        {
            OgImage = null,
            Language = new Language { Code = "en" }
        };
        var entityWithEmptyOgImage = new PageTranslation
        {
            OgImage = "",
            Language = new Language { Code = "en" }
        };
        var entityWithWhitespaceOgImage = new PageTranslation
        {
            OgImage = "  ",
            Language = new Language { Code = "en" }
        };

        // Act
        var dtoNull = _mapper.MapToDto(entityWithNullOgImage);
        var dtoEmpty = _mapper.MapToDto(entityWithEmptyOgImage);
        var dtoWhitespace = _mapper.MapToDto(entityWithWhitespaceOgImage);

        // Assert
        dtoNull.OgImage.Should().BeEmpty();
        dtoEmpty.OgImage.Should().BeEmpty();
        dtoWhitespace.OgImage.Should().BeEmpty();

        _urlBuilderMock.Verify(u => u.BuildUrl(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void MapToDtoList_MapsAllEntities()
    {
        // Arrange
        var entities = new List<PageTranslation>
        {
            new PageTranslation
            {
                Id = Guid.NewGuid(),
                Language = new Language { Code = "en" },
                PageId = Guid.NewGuid(),
                Title = "Title1",
                OgImage = "img1.png"
            },
            new PageTranslation
            {
                Id = Guid.NewGuid(),
                Language = new Language { Code = "fr" },
                PageId = Guid.NewGuid(),
                Title = "Title2",
                OgImage = null
            }
        };

        _urlBuilderMock.Setup(u => u.BuildUrl("img1.png")).Returns("url1");

        // Act
        var dtos = _mapper.MapToDtoList(entities);

        // Assert
        dtos.Should().HaveCount(2);
        dtos[0].OgImage.Should().Be("url1");
        dtos[0].Title.Should().Be("Title1");
        dtos[1].OgImage.Should().BeEmpty();
        dtos[1].Title.Should().Be("Title2");

        _urlBuilderMock.Verify(u => u.BuildUrl("img1.png"), Times.Once);
        _urlBuilderMock.Verify(u => u.BuildUrl(It.Is<string>(s => s != "img1.png")), Times.Never);
    }
}