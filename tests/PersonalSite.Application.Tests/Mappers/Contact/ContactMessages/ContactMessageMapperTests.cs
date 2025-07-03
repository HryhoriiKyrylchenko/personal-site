using PersonalSite.Application.Features.Contact.ContactMessages.Mappers;
using PersonalSite.Domain.Entities.Contact;

namespace PersonalSite.Application.Tests.Mappers.Contact.ContactMessages;

public class ContactMessageMapperTests
{
    private readonly ContactMessageMapper _mapper = new();

    [Fact]
    public void MapToDto_Should_Map_All_Fields_Correctly()
    {
        // Arrange
        var entity = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Subject here",
            Message = "Message content",
            CreatedAt = DateTime.UtcNow,
            IsRead = true
        };

        // Act
        var dto = _mapper.MapToDto(entity);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(entity.Id);
        dto.Name.Should().Be(entity.Name);
        dto.Email.Should().Be(entity.Email);
        dto.Subject.Should().Be(entity.Subject);
        dto.Message.Should().Be(entity.Message);
        dto.CreatedAt.Should().Be(entity.CreatedAt);
        dto.IsRead.Should().Be(entity.IsRead);
    }

    [Fact]
    public void MapToDtoList_Should_Map_All_Entities_To_Dtos()
    {
        // Arrange
        var entities = new List<ContactMessage>
        {
            new ContactMessage
            {
                Id = Guid.NewGuid(),
                Name = "User 1",
                Email = "user1@example.com",
                Subject = "Subj 1",
                Message = "Message 1",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            },
            new ContactMessage
            {
                Id = Guid.NewGuid(),
                Name = "User 2",
                Email = "user2@example.com",
                Subject = "Subj 2",
                Message = "Message 2",
                CreatedAt = DateTime.UtcNow,
                IsRead = true
            }
        };

        // Act
        var dtos = _mapper.MapToDtoList(entities);

        // Assert
        dtos.Should().HaveCount(entities.Count);
        dtos.Select(d => d.Id).Should().BeEquivalentTo(entities.Select(e => e.Id));
        dtos[0].Name.Should().Be(entities[0].Name);
        dtos[1].Name.Should().Be(entities[1].Name);
    }
}