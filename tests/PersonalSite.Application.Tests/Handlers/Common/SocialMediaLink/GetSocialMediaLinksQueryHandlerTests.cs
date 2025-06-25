using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinks;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.SocialMediaLink;

public class GetSocialMediaLinksQueryHandlerTests
{
    private readonly Mock<ISocialMediaLinkRepository> _repositoryMock = new();
    private readonly Mock<ILogger<GetSocialMediaLinksQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.SocialMediaLink, SocialMediaLinkDto>> _mapperMock = new();

    private readonly GetSocialMediaLinksQueryHandler _handler;

    public GetSocialMediaLinksQueryHandlerTests()
    {
        _handler = new GetSocialMediaLinksQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataExists()
    {
        // Arrange
        var socialLinks = new List<Domain.Entities.Common.SocialMediaLink>
        {
            CommonTestDataFactory.CreateSocialMediaLink()
        };
        var dtos = new List<SocialMediaLinkDto>
        {
            CommonTestDataFactory.MapToDto(socialLinks[0])
        };

        _repositoryMock.Setup(r => r.GetFilteredAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(PaginatedResult<Domain.Entities.Common.SocialMediaLink>.Success(
                value: socialLinks,
                pageNumber: 1,
                pageSize: 10,
                totalCount: socialLinks.Count));


        _mapperMock.Setup(m => m.MapToDtoList(socialLinks)).Returns(dtos);

        // Act
        var result = await _handler.Handle(new GetSocialMediaLinksQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryReturnsFailure()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetFilteredAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(PaginatedResult<Domain.Entities.Common.SocialMediaLink>.Failure("Social media links not found"));

        // Act
        var result = await _handler.Handle(new GetSocialMediaLinksQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Social media links not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetFilteredAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(new GetSocialMediaLinksQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to get social media links");
    }
}