using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Mappers;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Pages.Page;

public class PageMapperTests
{
    private readonly Mock<IS3UrlBuilder> _urlBuilderMock;
    private readonly Mock<IMapper<PageTranslation, PageTranslationDto>> _translationMapperMock;
    private readonly PageMapper _mapper;

    public PageMapperTests()
    {
        _urlBuilderMock = new Mock<IS3UrlBuilder>();
        _translationMapperMock = new Mock<IMapper<PageTranslation, PageTranslationDto>>();

        _mapper = new PageMapper(_urlBuilderMock.Object, _translationMapperMock.Object);
    }

    [Fact]
    public void MapToDto_WithMatchingTranslation_ReturnsMappedDto()
    {
        // Arrange
        var languageCode = "en";
        var ogImageKey = "image.png";
        var ogImageUrl = "https://s3.amazonaws.com/bucket/image.png";

        var translation = new PageTranslation
        {
            Language = new Language { Code = languageCode },
            Data = new Dictionary<string, string> { { "key", "value" } },
            Title = "Title",
            Description = "Description",
            MetaTitle = "Meta Title",
            MetaDescription = "Meta Description",
            OgImage = ogImageKey
        };

        var page = new Domain.Entities.Pages.Page
        {
            Id = Guid.NewGuid(),
            Key = "page-key",
            Translations = new List<PageTranslation> { translation }
        };

        _urlBuilderMock.Setup(u => u.BuildUrl(ogImageKey)).Returns(ogImageUrl);

        // Act
        var dto = _mapper.MapToDto(page, languageCode);

        // Assert
        dto.Id.Should().Be(page.Id);
        dto.Key.Should().Be(page.Key);
        dto.Data.Should().BeEquivalentTo(translation.Data);
        dto.Title.Should().Be(translation.Title);
        dto.Description.Should().Be(translation.Description);
        dto.MetaTitle.Should().Be(translation.MetaTitle);
        dto.MetaDescription.Should().Be(translation.MetaDescription);
        dto.OgImage.Should().Be(ogImageUrl);

        _urlBuilderMock.Verify(u => u.BuildUrl(ogImageKey), Times.Once);
    }

    [Fact]
    public void MapToDto_WithNoMatchingTranslation_ReturnsDtoWithDefaults()
    {
        // Arrange
        var languageCode = "fr"; // no matching translation
        var page = new Domain.Entities.Pages.Page
        {
            Id = Guid.NewGuid(),
            Key = "page-key",
            Translations = new List<PageTranslation>
            {
                new PageTranslation
                {
                    Language = new Language { Code = "en" },
                    Data = new Dictionary<string, string> { { "key", "value" } },
                    Title = "Title"
                }
            }
        };

        // Act
        var dto = _mapper.MapToDto(page, languageCode);

        // Assert
        dto.Id.Should().Be(page.Id);
        dto.Key.Should().Be(page.Key);
        dto.Data.Should().BeEmpty();
        dto.Title.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
        dto.MetaTitle.Should().BeEmpty();
        dto.MetaDescription.Should().BeEmpty();
        dto.OgImage.Should().BeEmpty();

        _urlBuilderMock.Verify(u => u.BuildUrl(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void MapToDto_WithEmptyOgImage_ReturnsEmptyOgImage()
    {
        // Arrange
        var languageCode = "en";
        var translation = new PageTranslation
        {
            Language = new Language { Code = languageCode },
            OgImage = "" // empty OgImage
        };
        var page = new Domain.Entities.Pages.Page
        {
            Id = Guid.NewGuid(),
            Key = "page-key",
            Translations = new List<PageTranslation> { translation }
        };

        // Act
        var dto = _mapper.MapToDto(page, languageCode);

        // Assert
        dto.OgImage.Should().BeEmpty();
        _urlBuilderMock.Verify(u => u.BuildUrl(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void MapToDtoList_ReturnsListOfDtos()
    {
        // Arrange
        var pages = new List<Domain.Entities.Pages.Page>
        {
            new Domain.Entities.Pages.Page
            {
                Id = Guid.NewGuid(),
                Key = "page1",
                Translations = new List<PageTranslation>
                {
                    new PageTranslation
                    {
                        Language = new Language { Code = "en" },
                        Title = "Title1"
                    }
                }
            },
            new Domain.Entities.Pages.Page
            {
                Id = Guid.NewGuid(),
                Key = "page2",
                Translations = new List<PageTranslation>
                {
                    new PageTranslation
                    {
                        Language = new Language { Code = "en" },
                        Title = "Title2"
                    }
                }
            }
        };

        _urlBuilderMock.Setup(u => u.BuildUrl(It.IsAny<string>())).Returns<string>(s => s);

        // Act
        var dtos = _mapper.MapToDtoList(pages, "en");

        // Assert
        dtos.Should().HaveCount(2);
        dtos.Select(d => d.Title).Should().Contain(new[] { "Title1", "Title2" });
    }

    [Fact]
    public void MapToAdminDto_ReturnsMappedAdminDto()
    {
        // Arrange
        var translations = new List<PageTranslation>
        {
            new PageTranslation { Id = Guid.NewGuid() },
            new PageTranslation { Id = Guid.NewGuid() }
        };

        var page = new Domain.Entities.Pages.Page
        {
            Id = Guid.NewGuid(),
            Key = "page-key",
            Translations = translations
        };

        var translationDtos = new List<PageTranslationDto>
        {
            new PageTranslationDto(),
            new PageTranslationDto()
        };

        _translationMapperMock
            .Setup(m => m.MapToDtoList(translations))
            .Returns(translationDtos);

        // Act
        var adminDto = _mapper.MapToAdminDto(page);

        // Assert
        adminDto.Id.Should().Be(page.Id);
        adminDto.Key.Should().Be(page.Key);
        adminDto.Translations.Should().BeEquivalentTo(translationDtos);
    }

    [Fact]
    public void MapToAdminDtoList_ReturnsListOfAdminDtos()
    {
        // Arrange
        var pages = new List<Domain.Entities.Pages.Page>
        {
            new Domain.Entities.Pages.Page
            {
                Id = Guid.NewGuid(),
                Key = "page1",
                Translations = new List<PageTranslation>()
            },
            new Domain.Entities.Pages.Page
            {
                Id = Guid.NewGuid(),
                Key = "page2",
                Translations = new List<PageTranslation>()
            }
        };

        // Setup _translationMapperMock to return empty list for any input
        _translationMapperMock
            .Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<PageTranslation>>()))
            .Returns(new List<PageTranslationDto>());

        // Act
        var adminDtos = _mapper.MapToAdminDtoList(pages);

        // Assert
        adminDtos.Should().HaveCount(2);
        adminDtos.Select(d => d.Key).Should().Contain(new[] { "page1", "page2" });
    }
}