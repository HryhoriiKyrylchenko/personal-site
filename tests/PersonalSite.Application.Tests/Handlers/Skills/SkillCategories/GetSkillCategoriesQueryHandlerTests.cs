using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.SkillCategories;

public class GetSkillCategoriesQueryHandlerTests
{
    private readonly Mock<ISkillCategoryRepository> _repositoryMock = new();
    private readonly Mock<IAdminMapper<SkillCategory, SkillCategoryAdminDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetSkillCategoriesQueryHandler>> _loggerMock = new();

    private readonly GetSkillCategoriesQueryHandler _handler;

    public GetSkillCategoriesQueryHandlerTests()
    {
        _handler = new GetSkillCategoriesQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenCategoriesFound()
    {
        // Arrange
        var category = SkillsTestDataFactory.CreateSkillCategory();
        var dto = SkillsTestDataFactory.MapToAdminDto(category);

        _repositoryMock.Setup(r =>
                r.GetFilteredAsync(It.IsAny<string>(), It.IsAny<short?>(), It.IsAny<short?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SkillCategory> { category });

        _mapperMock.Setup(m => m.MapToAdminDtoList(It.IsAny<List<SkillCategory>>()))
            .Returns(new List<SkillCategoryAdminDto> { dto });

        var query = new GetSkillCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value![0].Key.Should().Be(category.Key);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenNoCategoriesFound()
    {
        // Arrange
        _repositoryMock.Setup(r =>
                r.GetFilteredAsync(It.IsAny<string>(), It.IsAny<short?>(), It.IsAny<short?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SkillCategory>());

        var query = new GetSkillCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill categories not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r =>
                r.GetFilteredAsync(It.IsAny<string>(), It.IsAny<short?>(), It.IsAny<short?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var query = new GetSkillCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error while retrieving skill categories.");
    }
}
