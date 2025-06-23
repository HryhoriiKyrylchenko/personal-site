using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Tests.Handlers.Projects.Project;

public class GetProjectsQueryHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ILogger<GetProjectsQueryHandler>> _loggerMock;
    private readonly Mock<IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto>> _mapperMock;
    private readonly GetProjectsQueryHandler _handler;

    public GetProjectsQueryHandlerTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _loggerMock = new Mock<ILogger<GetProjectsQueryHandler>>();
        _mapperMock = new Mock<IAdminMapper<Domain.Entities.Projects.Project, ProjectAdminDto>>();

        _handler = new GetProjectsQueryHandler(
            _projectRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenProjectsFound()
    {
        // Arrange
        var query = new GetProjectsQuery();
        var project = ProjectTestDataFactory.CreateProject();
        var dto = ProjectTestDataFactory.MapToAdminDto(project);

        var paginatedResult = PaginatedResult<Domain.Entities.Projects.Project>.Success(
            [project], 1, 10, 1);

        _projectRepositoryMock
            .Setup(r => r.GetFilteredAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        _mapperMock
            .Setup(m => m.MapToAdminDtoList(It.Is<IEnumerable<Domain.Entities.Projects.Project>>(l => l.Contains(project))))
            .Returns([dto]);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Id.Should().Be(project.Id);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenRepositoryFails()
    {
        // Arrange
        var query = new GetProjectsQuery();
        var failureResult = PaginatedResult<Domain.Entities.Projects.Project>.Failure("Projects not found.");

        _projectRepositoryMock
            .Setup(r => r.GetFilteredAsync(query.Page, query.PageSize, query.SlugFilter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Projects not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var query = new GetProjectsQuery();

        _projectRepositoryMock
            .Setup(r => r.GetFilteredAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database down"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting projects.");
    }
}
