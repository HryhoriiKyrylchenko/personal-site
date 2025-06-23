using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Tests.Handlers.Contact.ContactMessages;

public class GetContactMessagesQueryHandlerTests
{
    private readonly Mock<IContactMessageRepository> _repositoryMock = new();
    private readonly Mock<ILogger<GetContactMessagesQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper<ContactMessage, ContactMessageDto>> _mapperMock = new();

    private GetContactMessagesQueryHandler CreateHandler() =>
        new(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenMessagesExist()
    {
        // Arrange
        var messages = new List<ContactMessage>
        {
            ContactTestDataFactory.CreateContactMessage(name:"John Doe"),
            ContactTestDataFactory.CreateContactMessage(name:"Jane Smith")
        };

        var pagedResult = PaginatedResult<ContactMessage>.Success(messages, 1, 10, 2);
        var dtoList = new List<ContactMessageDto>
        {
            ContactTestDataFactory.MapToDto(messages[0]),
            ContactTestDataFactory.MapToDto(messages[1])       
        };

        _repositoryMock.Setup(r => r.GetFilteredAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);
        _mapperMock.Setup(m => m.MapToDtoList(messages))
            .Returns(dtoList);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetContactMessagesQuery(1, 10), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtoList);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryReturnsFailure()
    {
        // Arrange
        var pagedResult = PaginatedResult<ContactMessage>.Failure("Not found");
        _repositoryMock.Setup(r => r.GetFilteredAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetContactMessagesQuery(1, 10), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Contact messages not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetFilteredAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database failure"));

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetContactMessagesQuery(1, 10), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error getting contact messages.");
    }
}