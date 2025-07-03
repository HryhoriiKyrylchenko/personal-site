using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.SiteInfo.Queries.GetSiteInfo;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Application.Tests.Common;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.SiteInfo;

public class GetSiteInfoQueryHandlerTests
{
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<ISocialMediaLinkRepository> _socialLinkRepoMock = new();
    private readonly Mock<IResumeRepository> _resumeRepoMock = new();
    private readonly Mock<ILogger<GetSiteInfoQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.Language, LanguageDto>> _languageMapperMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.SocialMediaLink, SocialMediaLinkDto>> _socialLinkMapperMock = new();
    private readonly Mock<IMapper<Domain.Entities.Common.Resume, ResumeDto>> _resumeMapperMock = new();

    private GetSiteInfoQueryHandler CreateHandler() => new(
        _languageRepoMock.Object,
        _socialLinkRepoMock.Object,
        _resumeRepoMock.Object,
        _loggerMock.Object,
        _languageMapperMock.Object,
        _socialLinkMapperMock.Object,
        _resumeMapperMock.Object);

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoLanguagesFound()
    {
        _languageRepoMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Common.Language>());

        var handler = CreateHandler();

        var result = await handler.Handle(new GetSiteInfoQuery(), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("No languages found.");

        _loggerMock.VerifyLog(LogLevel.Warning, "No languages found.", Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WithMappedData()
    {
        var languages = new List<Domain.Entities.Common.Language>
        {
            CommonTestDataFactory.CreateLanguage(),
            CommonTestDataFactory.CreateLanguage("pl")
        };

        var socialLinks = new List<Domain.Entities.Common.SocialMediaLink>
        {
            CommonTestDataFactory.CreateSocialMediaLink(),
        };

        var resumeEntity = CommonTestDataFactory.CreateResume(
            fileUrl: "resume.pdf",
            fileName: "Resume.pdf",
            isActive: true);

        var languageDtos = new List<LanguageDto>
        {
            CommonTestDataFactory.MapToDto(languages[0]),
            CommonTestDataFactory.MapToDto(languages[1])
        };

        var socialLinkDtos = new List<SocialMediaLinkDto>
        {
            CommonTestDataFactory.MapToDto(socialLinks[0])
        };

        var resumeDto = CommonTestDataFactory.MapToResumeDto(resumeEntity);
            
        _languageRepoMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);
        _socialLinkRepoMock.Setup(x => x.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(socialLinks);
        _resumeRepoMock.Setup(x => x.GetLastActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(resumeEntity);

        _languageMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<Domain.Entities.Common.Language>>()))
            .Returns(languageDtos);
        _socialLinkMapperMock.Setup(m => m.MapToDtoList(socialLinks))
            .Returns(socialLinkDtos);
        _resumeMapperMock.Setup(m => m.MapToDto(resumeEntity))
            .Returns(resumeDto);

        var handler = CreateHandler();

        var result = await handler.Handle(new GetSiteInfoQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value?.Languages.Should().BeEquivalentTo(languageDtos);
        result.Value?.SocialLinks.Should().BeEquivalentTo(socialLinkDtos);
        result.Value?.Resume.Should().BeEquivalentTo(resumeDto);
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenNoSocialLinksFound()
    {
        var languages = new List<Domain.Entities.Common.Language>
        {
            CommonTestDataFactory.CreateLanguage()
        };

        _languageRepoMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);
        _socialLinkRepoMock.Setup(x => x.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Common.SocialMediaLink>());
        _resumeRepoMock.Setup(x => x.GetLastActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Resume?)null);

        _languageMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<Domain.Entities.Common.Language>>()))
            .Returns([CommonTestDataFactory.MapToDto(languages[0])]);
        _socialLinkMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<Domain.Entities.Common.SocialMediaLink>>()))
            .Returns([]);

        var handler = CreateHandler();

        var result = await handler.Handle(new GetSiteInfoQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value?.SocialLinks.Should().BeEmpty();
        result.Value?.Resume.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        _languageRepoMock.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        var handler = CreateHandler();

        var result = await handler.Handle(new GetSiteInfoQuery(), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}