using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.SocialMediaLink;

public class DeleteSocialMediaLinkCommandHandlerTests
{
    private readonly Mock<ISocialMediaLinkRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteSocialMediaLinkCommandHandler _handler;

    public DeleteSocialMediaLinkCommandHandlerTests()
    {
        _repositoryMock = new Mock<ISocialMediaLinkRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var loggerMock = new Mock<ILogger<DeleteSocialMediaLinkCommandHandler>>();

        _handler = new DeleteSocialMediaLinkCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenEntityFoundAndDeleted()
    {
        // Arrange
        var entity = CommonTestDataFactory.CreateSocialMediaLink();

        _repositoryMock.Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(new DeleteSocialMediaLinkCommand(entity.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(entity), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEntityNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.SocialMediaLink?)null);

        // Act
        var result = await _handler.Handle(new DeleteSocialMediaLinkCommand(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Social media link not found.");

        _repositoryMock.Verify(r => r.Remove(It.IsAny<Domain.Entities.Common.SocialMediaLink>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldLogError_AndReturnFailure_OnException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var result = await _handler.Handle(new DeleteSocialMediaLinkCommand(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}