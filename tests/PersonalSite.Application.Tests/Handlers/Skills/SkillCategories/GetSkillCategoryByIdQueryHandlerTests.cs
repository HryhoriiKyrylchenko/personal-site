using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.SkillCategories;

public class GetSkillCategoryByIdQueryHandlerTests
{
    private readonly Mock<ISkillCategoryRepository> _repositoryMock = new();
    private readonly Mock<IAdminMapper<SkillCategory, SkillCategoryAdminDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetSkillCategoryByIdQueryHandler>> _loggerMock = new();

    private readonly GetSkillCategoryByIdQueryHandler _handler;

    public GetSkillCategoryByIdQueryHandlerTests()
    {
        _handler = new GetSkillCategoryByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenCategoryExists()
    {
        // Arrange
        var category = SkillsTestDataFactory.CreateSkillCategory();
        var dto = SkillsTestDataFactory.MapToAdminDto(category);

        _repositoryMock.Setup(r => r.GetWithTranslationsByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _mapperMock.Setup(m => m.MapToAdminDto(category)).Returns(dto);

        var query = new GetSkillCategoryByIdQuery(category.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWithTranslationsByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SkillCategory?)null);

        var query = new GetSkillCategoryByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill category not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWithTranslationsByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Something failed"));

        var query = new GetSkillCategoryByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error getting skill category by id.");
    }
}