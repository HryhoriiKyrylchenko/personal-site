using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Application.Features.Projects.Project.Commands.DeleteProject;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Tests.Handlers.Projects.Project;

public class DeleteProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<CreateProjectCommandHandler>> _loggerMock = new();

    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _handler = new DeleteProjectCommandHandler(
            _projectRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenProjectNotFound()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Projects.Project?)null);

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be($"Project with ID {projectId} not found.");
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenProjectDeleted()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Domain.Entities.Projects.Project { Id = projectId };

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        project.IsDeleted.Should().BeTrue();
        project.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _projectRepositoryMock.Verify(r => r.UpdateAsync(project, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        var command = new DeleteProjectCommand(projectId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error deleting project.");
    }
}