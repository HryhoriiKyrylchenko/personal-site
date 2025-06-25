using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.UpdateSocialMediaLink;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.SocialMediaLink;

public class UpdateSocialMediaLinkCommandHandlerTests
{
    private readonly Mock<ISocialMediaLinkRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateSocialMediaLinkCommandHandler _handler;

    public UpdateSocialMediaLinkCommandHandlerTests()
    {
        _repositoryMock = new Mock<ISocialMediaLinkRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var loggerMock = new Mock<ILogger<UpdateSocialMediaLinkCommandHandler>>();

        _handler = new UpdateSocialMediaLinkCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenEntityExistsAndUpdated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = CommonTestDataFactory.CreateSocialMediaLink(
            id: id,
            platform: "old",
            url: "old-url",
            displayOrder: 1,
            isActive: false);

        var command = CommonTestDataFactory.CreateUpdateSocialMediaLinkCommand(
            id: id,
            platform: "LinkedIn",
            url: "https://linkedin.com/in/me",
            displayOrder: 2,
            isActive: true
        );

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        entity.Platform.Should().Be(command.Platform);
        entity.Url.Should().Be(command.Url);
        entity.DisplayOrder.Should().Be(command.DisplayOrder);
        entity.IsActive.Should().Be(command.IsActive);

        _repositoryMock.Verify(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEntityNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.SocialMediaLink?)null);

        var command = CommonTestDataFactory.CreateUpdateSocialMediaLinkCommand(
            id: id,
            platform: "Twitter",
            url: "https://twitter.com",
            displayOrder: 1,
            isActive: true
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Social media link not found.");

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Common.SocialMediaLink>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldLogError_AndReturnFailure_OnException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = CommonTestDataFactory.CreateUpdateSocialMediaLinkCommand(
            id: id,
            platform: "YouTube",
            url: "https://youtube.com",
            displayOrder: 5,
            isActive: true
        );

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB connection failed"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}