using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Language.Queries.GetLanguages;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Language;

public class GetLanguagesQueryHandlerTests
{
    private readonly Mock<ILanguageRepository> _repositoryMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.Language, LanguageDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetLanguagesQueryHandler>> _loggerMock = new();
    private readonly GetLanguagesQueryHandler _handler;

    public GetLanguagesQueryHandlerTests()
    {
        _handler = new GetLanguagesQueryHandler(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithLanguages()
    {
        // Arrange
        var languages = new List<Domain.Entities.Common.Language>
        {
            CommonTestDataFactory.CreateLanguage(),
            CommonTestDataFactory.CreateLanguage("fr")
        };

        var languageDtos = languages.Select(
            CommonTestDataFactory.MapToDto).ToList();

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);

        _mapperMock.Setup(m => m.MapToDtoList(languages))
            .Returns(languageDtos);

        var query = new GetLanguagesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(languageDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var exception = new Exception("Something went wrong");
        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var query = new GetLanguagesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}
