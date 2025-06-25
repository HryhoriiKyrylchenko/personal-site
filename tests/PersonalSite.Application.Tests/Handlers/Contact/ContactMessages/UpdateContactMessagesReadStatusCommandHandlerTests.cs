using PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Tests.Handlers.Contact.ContactMessages;

public class UpdateContactMessagesReadStatusCommandHandlerTests
{
    private readonly Mock<IContactMessageRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<UpdateContactMessagesReadStatusCommandHandler>> _loggerMock;
    private readonly UpdateContactMessagesReadStatusCommandHandler _handler;

    public UpdateContactMessagesReadStatusCommandHandlerTests()
    {
        _repositoryMock = new Mock<IContactMessageRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdateContactMessagesReadStatusCommandHandler>>();
        _handler = new UpdateContactMessagesReadStatusCommandHandler(
            _repositoryMock.Object, 
            _unitOfWorkMock.Object, 
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAllMessages_WhenAllFound()
    {
        // Arrange
        var messageIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var messages = messageIds.Select(id => new ContactMessage { Id = id, IsRead = false }).ToList();

        _repositoryMock.Setup(r => r.GetByIdsAsync(messageIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        var command = ContactTestDataFactory.CreateUpdateContactMessagesReadStatusCommand(messageIds);

        var handler = new UpdateContactMessagesReadStatusCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        foreach (var msg in messages)
            msg.IsRead.Should().BeTrue();

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ContactMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(messages.Count));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSomeMessagesNotFound()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var messages = new List<ContactMessage> { new ContactMessage { Id = ids[0], IsRead = false } }; // Only 1 found
        var command = ContactTestDataFactory.CreateUpdateContactMessagesReadStatusCommand(ids);

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Some messages were not found.");
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ContactMessage>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldLogErrorAndReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid() };
        var command =ContactTestDataFactory.CreateUpdateContactMessagesReadStatusCommand(ids);

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Failed to update read status.");
    }
}