using PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Handlers.Common.Resume;

public class CreateResumeCommandHandlerTests
{
    private readonly Mock<IResumeRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateResumeCommandHandler>> _loggerMock;
    private readonly CreateResumeCommandHandler _handler;
    

    public CreateResumeCommandHandlerTests()
    {
        _repositoryMock = new Mock<IResumeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateResumeCommandHandler>>();
        var urlBuilderMock = new Mock<IS3UrlBuilder>();

        _handler = new CreateResumeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            urlBuilderMock.Object);
        
        
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithNewGuid_WhenResumeCreated()
    {
        // Arrange
        var command = CommonTestDataFactory.CreateCreateResumeCommand();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Common.Resume>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _repositoryMock.Verify(r => r.AddAsync(
                It.Is<Domain.Entities.Common.Resume>(res => 
                    res.FileUrl == command.FileUrl &&
                    res.FileName == command.FileName &&
                    res.IsActive == command.IsActive),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_AndLogError_WhenExceptionThrown()
    {
        // Arrange
        var command = CommonTestDataFactory.CreateCreateResumeCommand("url", "file");
        var exception = new Exception("Database error");

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Common.Resume>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Failed to create resume.");
    }
}