using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Resume;

public class GetResumesQueryHandlerTests
{
    private readonly Mock<IResumeRepository> _repositoryMock;
    private readonly Mock<ILogger<GetResumesQueryHandler>> _loggerMock;
    private readonly Mock<IMapper<Domain.Entities.Common.Resume, ResumeDto>> _mapperMock;
    private readonly GetResumesQueryHandler _handler;

    public GetResumesQueryHandlerTests()
    {
        _repositoryMock = new Mock<IResumeRepository>();
        _loggerMock = new Mock<ILogger<GetResumesQueryHandler>>();
        _mapperMock = new Mock<IMapper<Domain.Entities.Common.Resume, ResumeDto>>();

        _handler = new GetResumesQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenResumesFound()
    {
        // Arrange
        var resumes = new List<Domain.Entities.Common.Resume>
        {
            CommonTestDataFactory.CreateResume(),
            CommonTestDataFactory.CreateResume(),
            CommonTestDataFactory.CreateResume()
        };

        var paginatedResult = PaginatedResult<Domain.Entities.Common.Resume>.Success(
            resumes,
            pageNumber: 1,
            pageSize: 20,
            totalCount: 3);

        var resumeDtos = resumes.Select(CommonTestDataFactory.MapToResumeDto).ToList();

        _repositoryMock
            .Setup(r => r.GetFilteredAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        _mapperMock
            .Setup(m => m.MapToDtoList(resumes))
            .Returns(resumeDtos);

        var query = new GetResumesQuery(Page: 1, PageSize: 20, IsActive: true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(resumeDtos);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoResumesFound()
    {
        // Arrange
        var emptyResult = PaginatedResult<Domain.Entities.Common.Resume>.Failure("Resumes not found");

        _repositoryMock
            .Setup(r => r.GetFilteredAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyResult);

        var query = new GetResumesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Resumes not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var exception = new Exception("Some error");

        _repositoryMock
            .Setup(r => r.GetFilteredAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var query = new GetResumesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("An error occurred while getting the resumes.");
    }
}