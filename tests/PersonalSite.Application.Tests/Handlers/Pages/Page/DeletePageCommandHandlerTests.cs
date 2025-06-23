using PersonalSite.Application.Features.Pages.Page.Commands.DeletePage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class DeletePageCommandHandlerTests
    {
        private readonly Mock<IPageRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeletePageCommandHandler>> _loggerMock;
        private readonly DeletePageCommandHandler _handler;

        public DeletePageCommandHandlerTests()
        {
            _repositoryMock = new Mock<IPageRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeletePageCommandHandler>>();

            _handler = new DeletePageCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_DeletesExistingPage_ReturnsSuccess()
        {
            // Arrange
            var page = PageTestDataFactory.CreatePage();
            var command = new DeletePageCommand(page.Id);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(page.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);

            _repositoryMock
                .Setup(r => r.UpdateAsync(page, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            page.IsDeleted.Should().BeTrue();

            _repositoryMock.Verify(r => r.UpdateAsync(page, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_PageNotFound_ReturnsFailure()
        {
            // Arrange
            var command = new DeletePageCommand(Guid.NewGuid());

            _repositoryMock
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Pages.Page?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Page not found.");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Pages.Page>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsFailureAndLogsError()
        {
            // Arrange
            var page = PageTestDataFactory.CreatePage();
            var command = new DeletePageCommand(page.Id);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(page.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);

            _repositoryMock
                .Setup(r => r.UpdateAsync(page, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database failure"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Error deleting page.");

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error deleting page.") == true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }
    }