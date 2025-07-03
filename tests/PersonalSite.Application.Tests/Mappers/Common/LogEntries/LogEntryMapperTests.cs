using PersonalSite.Application.Features.Common.LogEntries.Mappers;
using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Application.Tests.Mappers.Common.LogEntries;

public class LogEntryMapperTests
{
    private readonly LogEntryMapper _mapper = new();

    [Fact]
    public void MapToDto_ShouldMapCorrectly()
    {
        // Arrange
        var logEntry = new LogEntry
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Level = "Error",
            Message = "Test error occurred",
            MessageTemplate = "{Message}",
            Exception = "StackTrace...",
            Properties = "{\"userId\":123}",
            SourceContext = "TestContext"
        };

        // Act
        var dto = _mapper.MapToDto(logEntry);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(logEntry.Id);
        dto.Timestamp.Should().Be(logEntry.Timestamp);
        dto.Level.Should().Be(logEntry.Level);
        dto.Message.Should().Be(logEntry.Message);
        dto.MessageTemplate.Should().Be(logEntry.MessageTemplate);
        dto.Exception.Should().Be(logEntry.Exception);
        dto.Properties.Should().Be(logEntry.Properties);
        dto.SourceContext.Should().Be(logEntry.SourceContext);
    }

    [Fact]
    public void MapToDtoList_ShouldMapListCorrectly()
    {
        // Arrange
        var logs = new List<LogEntry>
        {
            new LogEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Level = "Info",
                Message = "Info message",
                MessageTemplate = "{Info}",
                Exception = null,
                Properties = "{}",
                SourceContext = "App"
            },
            new LogEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Level = "Warning",
                Message = "Warn message",
                MessageTemplate = "{Warn}",
                Exception = null,
                Properties = "{}",
                SourceContext = "App"
            }
        };

        // Act
        var dtoList = _mapper.MapToDtoList(logs);

        // Assert
        dtoList.Should().HaveSameCount(logs);

        for (int i = 0; i < logs.Count; i++)
        {
            dtoList[i].Id.Should().Be(logs[i].Id);
            dtoList[i].Message.Should().Be(logs[i].Message);
        }
    }
}