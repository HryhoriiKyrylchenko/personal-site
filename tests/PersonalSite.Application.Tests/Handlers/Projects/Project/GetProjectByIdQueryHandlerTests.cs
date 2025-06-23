using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Projects.Project.Queries.GetProjectById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Tests.Handlers.Projects.Project;

public class GetProjectByIdQueryHandlerTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly Mock<IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto>> _mapperMock;
    private readonly Mock<ILogger<GetProjectByIdQueryHandler>> _loggerMock;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto>>();
        _loggerMock = new Mock<ILogger<GetProjectByIdQueryHandler>>();

        _handler = new GetProjectByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenProjectExists()
    {
        // Arrange
        var project = ProjectTestDataFactory.CreateProject();
        var dto = ProjectTestDataFactory.MapToAdminDto(project);

        _repositoryMock.Setup(r => r.GetWithFullDataAsync(project.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(project);
        _mapperMock.Setup(m => m.MapToAdminDto(project)).Returns(dto);

        var query = new GetProjectByIdQuery(project.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenProjectNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWithFullDataAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Domain.Entities.Projects.Project?)null);

        var query = new GetProjectByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Project not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWithFullDataAsync(id, It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("Database error"));

        var query = new GetProjectByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting project by id.");
    }
}