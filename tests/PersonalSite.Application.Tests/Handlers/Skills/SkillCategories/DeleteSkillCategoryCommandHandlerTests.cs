using PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.SkillCategories;

public class DeleteSkillCategoryCommandHandlerTests
{
    private readonly Mock<ISkillCategoryRepository> _repositoryMock = new();
    private readonly Mock<ISkillRepository> _skillRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteSkillCategoryCommandHandler>> _loggerMock = new();

    private readonly DeleteSkillCategoryCommandHandler _handler;

    public DeleteSkillCategoryCommandHandlerTests()
    {
        _handler = new DeleteSkillCategoryCommandHandler(
            _repositoryMock.Object,
            _skillRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenCategoryExists()
    {
        // Arrange
        var category = new SkillCategory { Id = Guid.NewGuid(), Key = "backend", DisplayOrder = 1 };
        var command = new DeleteSkillCategoryCommand(category.Id);

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(category), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryNotFound()
    {
        // Arrange
        var command = new DeleteSkillCategoryCommand(Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SkillCategory?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill category not found.");
        _repositoryMock.Verify(r => r.Remove(It.IsAny<SkillCategory>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var command = new DeleteSkillCategoryCommand(Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error occurred while deleting skill category.");
    }
}