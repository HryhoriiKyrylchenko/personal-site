using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.LogEntries;

public class GetLogEntriesQueryHandlerTests
{
    private readonly Mock<ILogEntryRepository> _repositoryMock = new();
    private readonly Mock<ILogger<GetLogEntriesQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper<LogEntry, LogEntryDto>> _mapperMock = new();
    private readonly GetLogEntriesQueryHandler _handler;

    public GetLogEntriesQueryHandlerTests()
    {
        _handler = new GetLogEntriesQueryHandler(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenLogsFound()
    {
        // Arrange
        var query = new GetLogEntriesQuery();

        var logEntities = new List<LogEntry>
        {
            CommonTestDataFactory.CreateLogEntry(),
            CommonTestDataFactory.CreateLogEntry()
        };

        var pagedResult = PaginatedResult<LogEntry>.Success(logEntities, 1, 20, 2);

        _repositoryMock.Setup(r => r.GetFilteredAsync(
                query.Page, query.PageSize, query.LevelFilter, query.SourceContextFilter, query.From, query.To, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        var mappedDtos = logEntities.Select(e => new LogEntryDto
        {
            Id = e.Id,
            Timestamp = e.Timestamp,
            Level = e.Level,
            Message = e.Message,
            MessageTemplate = e.MessageTemplate,
            Exception = e.Exception,
            Properties = e.Properties,
            SourceContext = e.SourceContext
        }).ToList();

        _mapperMock.Setup(m => m.MapToDtoList(logEntities)).Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(mappedDtos);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.TotalCount.Should().Be(2);

        _repositoryMock.VerifyAll();
        _mapperMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoLogsFound()
    {
        // Arrange
        var query = new GetLogEntriesQuery();

        var pagedResult = PaginatedResult<LogEntry>.Failure("Failed to get logs");

        _repositoryMock.Setup(r => r.GetFilteredAsync(
                query.Page, query.PageSize, query.LevelFilter, query.SourceContextFilter, query.From, query.To, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Log entries not found");

        _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Log entries not found")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var query = new GetLogEntriesQuery();

        _repositoryMock.Setup(r => r.GetFilteredAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some error"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting log entries.");

        _loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error getting log entries.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

    }
}