using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Tests.Handlers.Contact.ContactMessages;

public class GetContactMessageByIdQueryHandlerTests
{
    private readonly Mock<IContactMessageRepository> _repositoryMock = new();
    private readonly Mock<ILogger<GetContactMessageByIdQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper<ContactMessage, ContactMessageDto>> _mapperMock = new();

    private GetContactMessageByIdQueryHandler CreateHandler() =>
        new(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenContactMessageExists()
    {
        // Arrange
        var entity = ContactTestDataFactory.CreateContactMessage();
        var dto = ContactTestDataFactory.MapToDto(entity);

        _repositoryMock.Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        _mapperMock.Setup(m => m.MapToDto(entity)).Returns(dto);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetContactMessageByIdQuery(entity.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContactMessageNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContactMessage?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetContactMessageByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Contact message not found.");
    }

    [Fact]
    public async Task Handle_ShouldLogErrorAndReturnFailure_OnException()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("db error"));

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetContactMessageByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error getting contact message by id.");
    }
}