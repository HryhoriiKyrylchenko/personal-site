using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetAboutPage;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetAboutPageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<IUserSkillRepository> _userSkillRepositoryMock;
    private readonly Mock<ILearningSkillRepository> _learningSkillRepositoryMock;
    private readonly Mock<ILogger<GetAboutPageQueryHandler>> _loggerMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly Mock<ITranslatableMapper<LearningSkill, LearningSkillDto>> _learningSkillMapperMock;
    private readonly Mock<ITranslatableMapper<UserSkill, UserSkillDto>> _userSkillMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetAboutPageQueryHandler _handler;

    public GetAboutPageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        _userSkillRepositoryMock = new Mock<IUserSkillRepository>();
        _learningSkillRepositoryMock = new Mock<ILearningSkillRepository>();
        _loggerMock = new Mock<ILogger<GetAboutPageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();
        _learningSkillMapperMock = new Mock<ITranslatableMapper<LearningSkill, LearningSkillDto>>();
        _userSkillMapperMock = new Mock<ITranslatableMapper<UserSkill, UserSkillDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetAboutPageQueryHandler(
            _languageContext,
            _pageRepositoryMock.Object,
            _userSkillRepositoryMock.Object,
            _learningSkillRepositoryMock.Object,
            _loggerMock.Object,
            _pageMapperMock.Object,
            _learningSkillMapperMock.Object,
            _userSkillMapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_InvalidLanguageContext_ReturnsFailure()
    {
        // Arrange
        _languageContext.LanguageCode = null!;
        var query = new GetAboutPageQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid language context.");
    }

    [Fact]
    public async Task Handle_PageNotFound_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("about", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(new GetAboutPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Home page not found.");
    }

    [Fact]
    public async Task Handle_NoUserSkillsOrLearningSkills_LogsWarningAndReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto { Title = "About" };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("about", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock.Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        _userSkillRepositoryMock.Setup(r => r.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserSkill>());

        _userSkillMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<UserSkill>>(), "en"))
            .Returns(new List<UserSkillDto>());

        _learningSkillRepositoryMock.Setup(r => r.GetAllOrderedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LearningSkill>());

        _learningSkillMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<LearningSkill>>(), "en"))
            .Returns(new List<LearningSkillDto>());

        // Act
        var result = await _handler.Handle(new GetAboutPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.UserSkills.Should().BeEmpty();
        result.Value.LearningSkills.Should().BeEmpty();

        // Verify warning logs for no skills
        _loggerMock.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("No skills found.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2)); // Once for user skills and once for learning skills
    }

    [Fact]
    public async Task Handle_AllDataPresent_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto { Title = "About" };

        var userSkill = new UserSkill { Id = Guid.NewGuid() };
        var userSkillDto = new UserSkillDto { Id = userSkill.Id };

        var learningSkill = new LearningSkill { Id = Guid.NewGuid() };
        var learningSkillDto = new LearningSkillDto { Id = learningSkill.Id };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("about", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock.Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        _userSkillRepositoryMock.Setup(r => r.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserSkill> { userSkill });

        _userSkillMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<UserSkill>>(), "en"))
            .Returns(new List<UserSkillDto> { userSkillDto });

        _learningSkillRepositoryMock.Setup(r => r.GetAllOrderedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LearningSkill> { learningSkill });

        _learningSkillMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<LearningSkill>>(), "en"))
            .Returns(new List<LearningSkillDto> { learningSkillDto });

        // Act
        var result = await _handler.Handle(new GetAboutPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.UserSkills.Should().ContainSingle().Which.Id.Should().Be(userSkill.Id);
        result.Value.LearningSkills.Should().ContainSingle().Which.Id.Should().Be(learningSkill.Id);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailureAndLogsError()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(new GetAboutPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");

        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error occurred while getting About page data.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}