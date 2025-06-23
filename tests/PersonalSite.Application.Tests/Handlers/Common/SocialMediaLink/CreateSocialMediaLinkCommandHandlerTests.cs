using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.SocialMediaLink;

public class CreateSocialMediaLinkCommandHandlerTests
{
    private readonly Mock<ISocialMediaLinkRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateSocialMediaLinkCommandHandler>> _loggerMock;

    private readonly CreateSocialMediaLinkCommandHandler _handler;

    public CreateSocialMediaLinkCommandHandlerTests()
    {
        _repositoryMock = new Mock<ISocialMediaLinkRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateSocialMediaLinkCommandHandler>>();

        _handler = new CreateSocialMediaLinkCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCreationIsSuccessful()
    {
        // Arrange
        var command = new CreateSocialMediaLinkCommand(
            Platform: "GitHub",
            Url: "https://github.com/user",
            DisplayOrder: 1,
            IsActive: true
        );

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Common.SocialMediaLink>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Common.SocialMediaLink>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var command = new CreateSocialMediaLinkCommand(
            Platform: "Twitter",
            Url: "https://twitter.com/user",
            DisplayOrder: 2,
            IsActive: true
        );

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Common.SocialMediaLink>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to create social media link.");

        _loggerMock.Verify(
            x => x.Log<It.IsAnyType>(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error creating social media link")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }
}