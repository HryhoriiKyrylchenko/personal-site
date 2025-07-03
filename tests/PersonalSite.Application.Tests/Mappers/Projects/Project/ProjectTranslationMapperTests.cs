using PersonalSite.Application.Features.Projects.Project.Mappers;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Projects.Project;

public class ProjectTranslationMapperTests
{
    private readonly Mock<IS3UrlBuilder> _s3UrlBuilderMock = new();
    private readonly ProjectTranslationMapper _mapper;

    public ProjectTranslationMapperTests()
    {
        _mapper = new ProjectTranslationMapper(_s3UrlBuilderMock.Object);
    }

    [Fact]
    public void MapToDto_Should_Map_All_Fields_Correctly_With_OgImage()
    {
        // Arrange
        var ogImage = "image.jpg";
        var fullUrl = "https://s3.com/image.jpg";

        var entity = new ProjectTranslation
        {
            Id = Guid.NewGuid(),
            Language = new Language { Code = "en" },
            ProjectId = Guid.NewGuid(),
            Title = "Test Title",
            ShortDescription = "Short Desc",
            DescriptionSections = new Dictionary<string, string> { { "section", "text" } },
            MetaTitle = "Meta Title",
            MetaDescription = "Meta Description",
            OgImage = ogImage
        };

        _s3UrlBuilderMock.Setup(s => s.BuildUrl(ogImage)).Returns(fullUrl);

        // Act
        var dto = _mapper.MapToDto(entity);

        // Assert
        dto.Id.Should().Be(entity.Id);
        dto.LanguageCode.Should().Be("en");
        dto.ProjectId.Should().Be(entity.ProjectId);
        dto.Title.Should().Be(entity.Title);
        dto.ShortDescription.Should().Be(entity.ShortDescription);
        dto.DescriptionSections.Should().BeEquivalentTo(entity.DescriptionSections);
        dto.MetaTitle.Should().Be(entity.MetaTitle);
        dto.MetaDescription.Should().Be(entity.MetaDescription);
        dto.OgImage.Should().Be(fullUrl);
    }

    [Fact]
    public void MapToDto_Should_Return_Empty_OgImage_If_Null_Or_Whitespace()
    {
        // Arrange
        var entity = new ProjectTranslation
        {
            Id = Guid.NewGuid(),
            Language = new Language { Code = "en" },
            ProjectId = Guid.NewGuid(),
            OgImage = ""
        };

        // Act
        var dto = _mapper.MapToDto(entity);

        // Assert
        dto.OgImage.Should().BeEmpty();
        _s3UrlBuilderMock.Verify(s => s.BuildUrl(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void MapToDtoList_Should_Map_All_Items()
    {
        // Arrange
        var translation1 = new ProjectTranslation
        {
            Id = Guid.NewGuid(),
            Language = new Language { Code = "en" },
            ProjectId = Guid.NewGuid(),
            Title = "Title1",
            OgImage = "image1.jpg"
        };

        var translation2 = new ProjectTranslation
        {
            Id = Guid.NewGuid(),
            Language = new Language { Code = "fr" },
            ProjectId = Guid.NewGuid(),
            Title = "Title2",
            OgImage = ""
        };

        _s3UrlBuilderMock.Setup(s => s.BuildUrl("image1.jpg")).Returns("https://s3.com/image1.jpg");

        var entities = new List<ProjectTranslation> { translation1, translation2 };

        // Act
        var result = _mapper.MapToDtoList(entities);

        // Assert
        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Title1");
        result[0].OgImage.Should().Be("https://s3.com/image1.jpg");
        result[1].Title.Should().Be("Title2");
        result[1].OgImage.Should().BeEmpty();
    }
}