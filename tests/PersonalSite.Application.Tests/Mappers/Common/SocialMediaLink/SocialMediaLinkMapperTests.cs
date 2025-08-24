using PersonalSite.Application.Features.Common.SocialMediaLinks.Mappers;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Common.SocialMediaLink;

public class SocialMediaLinkMapperTests
    {
        private readonly SocialMediaLinkMapper _mapper;

        public SocialMediaLinkMapperTests()
        {
            _mapper = new SocialMediaLinkMapper();
        }

        [Fact]
        public void MapToDto_ShouldReturnDtoWithExpectedData()
        {
            // Arrange
            var entity = new Domain.Entities.Common.SocialMediaLink
            {
                Id = Guid.NewGuid(),
                Platform = "Twitter",
                Url = "media/twitter.png",
                DisplayOrder = 1,
                IsActive = true
            };

            // Act
            var dto = _mapper.MapToDto(entity);

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(entity.Id);
            dto.Platform.Should().Be(entity.Platform);
            dto.Url.Should().Be(entity.Url);
            dto.DisplayOrder.Should().Be(entity.DisplayOrder);
            dto.IsActive.Should().Be(entity.IsActive);
        }

        [Fact]
        public void MapToDtoList_ShouldReturnMappedDtoList()
        {
            // Arrange
            var entities = new List<Domain.Entities.Common.SocialMediaLink>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Platform = "LinkedIn",
                    Url = "media/linkedin.png",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Platform = "GitHub",
                    Url = "media/github.png",
                    DisplayOrder = 2,
                    IsActive = false
                }
            };

            // Act
            var dtos = _mapper.MapToDtoList(entities);

            // Assert
            dtos.Should().HaveCount(2);
            dtos[0].Platform.Should().Be("LinkedIn");
            dtos[0].Url.Should().Be("media/linkedin.png");  // <-- raw url
            dtos[1].Platform.Should().Be("GitHub");
            dtos[1].Url.Should().Be("media/github.png");    // <-- raw url
        }
    }