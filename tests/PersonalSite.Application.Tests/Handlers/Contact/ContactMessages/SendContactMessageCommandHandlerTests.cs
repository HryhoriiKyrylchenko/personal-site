using MediatR;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;
using PersonalSite.Application.Features.Contact.ContactMessages.Events.ContactMessageCreated;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Tests.Handlers.Contact.ContactMessages;

public class SendContactMessageCommandHandlerTests
{
    private readonly Mock<IContactMessageRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<ILogger<SendContactMessageCommandHandler>> _loggerMock = new();

    private SendContactMessageCommandHandler CreateHandler() =>
        new(_repositoryMock.Object, _unitOfWorkMock.Object, _mediatorMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnSuccess_AndPublishEvent_WhenValidRequest()
    {
        // Arrange
        var command = new SendContactMessageCommand("John", "john@example.com", "Hello", "Test message")
        {
            IpAddress = "127.0.0.1",
            UserAgent = "TestAgent"
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repositoryMock.Verify(r => r.AddAsync(It.Is<ContactMessage>(m =>
            m.Name == "John" &&
            m.Email == "john@example.com" &&
            m.Subject == "Hello" &&
            m.Message == "Test message" &&
            m.IpAddress == "127.0.0.1" &&
            m.UserAgent == "TestAgent" &&
            !m.IsRead
        ), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<ContactMessageCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var command = new SendContactMessageCommand("John", "john@example.com", "Hello", "Test message");
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<ContactMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error creating contact message.");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error creating contact message.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}