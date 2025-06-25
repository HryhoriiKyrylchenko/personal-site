using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Language.Queries.GetLanguageById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Language;

public class GetLanguageByIdQueryHandlerTests
{
    private readonly Mock<ILanguageRepository> _repositoryMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.Language, LanguageDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetLanguageByIdQueryHandler>> _loggerMock = new();
    private readonly GetLanguageByIdQueryHandler _handler;

    public GetLanguageByIdQueryHandlerTests()
    {
        _handler = new GetLanguageByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnLanguage_WhenLanguageExists()
    {
        // Arrange
        var language = CommonTestDataFactory.CreateLanguage("fr");
        var dto = CommonTestDataFactory.MapToDto(language);
        _repositoryMock.Setup(r => r.GetByIdAsync(language.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);
        _mapperMock.Setup(m => m.MapToDto(language)).Returns(dto);

        var query = new GetLanguageByIdQuery(language.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
        _repositoryMock.Verify(r => r.GetByIdAsync(language.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.MapToDto(language), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLanguageDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Language?)null);

        var query = new GetLanguageByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Language not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some failure"));

        var query = new GetLanguageByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting language by id.");
    }
}