using PersonalSite.Application.Features.Contact.ContactMessages.Commands.DeleteContactMessages;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Tests.Handlers.Contact.ContactMessages;

public class DeleteContactMessagesCommandHandlerTests
{
    private readonly Mock<IContactMessageRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteContactMessagesCommandHandler>> _loggerMock = new();

    private DeleteContactMessagesCommandHandler CreateHandler() =>
        new(_repositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAllMessagesExist()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var messages = new List<ContactMessage>
        {
            ContactTestDataFactory.CreateContactMessage(id: ids[0], name: "Test 1"),
            ContactTestDataFactory.CreateContactMessage(id: ids[1], name: "Test 2")
        };

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new DeleteContactMessagesCommand(ids), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(It.IsAny<ContactMessage>()), Times.Exactly(2));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSomeMessagesNotFound()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var onlyOneMessage = new List<ContactMessage>
        {
            ContactTestDataFactory.CreateContactMessage(id: ids[0], name: "Only found")
        };

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(onlyOneMessage);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new DeleteContactMessagesCommand(ids), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Some messages were not found.");
        _repositoryMock.Verify(r => r.Remove(It.IsAny<ContactMessage>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid() };
        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new DeleteContactMessagesCommand(ids), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to delete messages.");
    }
}