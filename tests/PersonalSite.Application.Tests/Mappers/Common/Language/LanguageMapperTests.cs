using PersonalSite.Application.Features.Common.Language.Mappers;

namespace PersonalSite.Application.Tests.Mappers.Common.Language;

public class LanguageMapperTests
{
    private readonly LanguageMapper _mapper = new();

    [Fact]
    public void MapToDto_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var language = new Domain.Entities.Common.Language
        {
            Id = Guid.NewGuid(),
            Code = "en",
            Name = "English"
        };

        // Act
        var dto = _mapper.MapToDto(language);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(language.Id);
        dto.Code.Should().Be(language.Code);
        dto.Name.Should().Be(language.Name);
    }

    [Fact]
    public void MapToDtoList_ShouldMapAllEntitiesCorrectly()
    {
        // Arrange
        var languages = new List<Domain.Entities.Common.Language>
        {
            new() { Id = Guid.NewGuid(), Code = "en", Name = "English" },
            new() { Id = Guid.NewGuid(), Code = "fr", Name = "French" }
        };

        // Act
        var dtos = _mapper.MapToDtoList(languages);

        // Assert
        dtos.Should().HaveCount(languages.Count);
        dtos.Select(d => d.Id).Should().BeEquivalentTo(languages.Select(l => l.Id));
        dtos.Select(d => d.Code).Should().BeEquivalentTo(languages.Select(l => l.Code));
        dtos.Select(d => d.Name).Should().BeEquivalentTo(languages.Select(l => l.Name));
    }
}
