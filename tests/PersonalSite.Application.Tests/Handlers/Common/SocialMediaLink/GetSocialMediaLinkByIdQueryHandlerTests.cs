using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinkById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.SocialMediaLink;

public class GetSocialMediaLinkByIdQueryHandlerTests
{
    private readonly Mock<ISocialMediaLinkRepository> _repositoryMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.SocialMediaLink, SocialMediaLinkDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetSocialMediaLinkByIdQueryHandler>> _loggerMock = new();

    private GetSocialMediaLinkByIdQueryHandler CreateHandler() =>
        new(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenLinkExists()
    {
        // Arrange
        var link = CommonTestDataFactory.CreateSocialMediaLink();
        var dto = CommonTestDataFactory.MapToDto(link);

        _repositoryMock.Setup(r => r.GetByIdAsync(link.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(link);

        _mapperMock.Setup(m => m.MapToDto(link)).Returns(dto);

        var handler = CreateHandler();
        var query = new GetSocialMediaLinkByIdQuery(link.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLinkNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Domain.Entities.Common.SocialMediaLink?)null);

        var handler = CreateHandler();
        var query = new GetSocialMediaLinkByIdQuery(id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Social media link not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("DB error"));

        var handler = CreateHandler();
        var query = new GetSocialMediaLinkByIdQuery(id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("DB error");
    }
}