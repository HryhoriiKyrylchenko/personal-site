using PersonalSite.Application.Features.Common.SocialMediaLinks.Mappers;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Common.SocialMediaLink;

public class SocialMediaLinkMapperTests
    {
        private readonly Mock<IS3UrlBuilder> _urlBuilderMock;
        private readonly SocialMediaLinkMapper _mapper;

        public SocialMediaLinkMapperTests()
        {
            _urlBuilderMock = new Mock<IS3UrlBuilder>();
            _mapper = new SocialMediaLinkMapper(_urlBuilderMock.Object);
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

            var expectedUrl = "https://cdn.site.com/media/twitter.png";

            _urlBuilderMock.Setup(x => x.BuildUrl(entity.Url)).Returns(expectedUrl);

            // Act
            var dto = _mapper.MapToDto(entity);

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(entity.Id);
            dto.Platform.Should().Be(entity.Platform);
            dto.Url.Should().Be(expectedUrl);
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

            _urlBuilderMock.Setup(x => x.BuildUrl(It.IsAny<string>()))
                .Returns<string>(url => $"https://cdn.site.com/{url}");

            // Act
            var dtos = _mapper.MapToDtoList(entities);

            // Assert
            dtos.Should().HaveCount(2);
            dtos[0].Platform.Should().Be("LinkedIn");
            dtos[0].Url.Should().Be("https://cdn.site.com/media/linkedin.png");
            dtos[1].Platform.Should().Be("GitHub");
            dtos[1].Url.Should().Be("https://cdn.site.com/media/github.png");
        }
    }