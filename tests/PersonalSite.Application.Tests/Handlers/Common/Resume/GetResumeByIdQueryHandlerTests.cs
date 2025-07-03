using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Resume;

public class GetResumeByIdQueryHandlerTests
{
    private readonly Mock<IResumeRepository> _repositoryMock;
    private readonly Mock<ILogger<GetResumeByIdQueryHandler>> _loggerMock;
    private readonly Mock<IMapper<Domain.Entities.Common.Resume, ResumeDto>> _mapperMock;
    private readonly GetResumeByIdQueryHandler _handler;

    public GetResumeByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IResumeRepository>();
        _loggerMock = new Mock<ILogger<GetResumeByIdQueryHandler>>();
        _mapperMock = new Mock<IMapper<Domain.Entities.Common.Resume, ResumeDto>>();

        _handler = new GetResumeByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnResume_WhenResumeExists()
    {
        // Arrange
        var resume = CommonTestDataFactory.CreateResume();
        var resumeDto = CommonTestDataFactory.MapToResumeDto(resume);
        var query = new GetResumeByIdQuery(resume.Id);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resume.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resume);

        _mapperMock
            .Setup(m => m.MapToDto(resume))
            .Returns(resumeDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(resumeDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenResumeNotFound()
    {
        // Arrange
        var query = new GetResumeByIdQuery(Guid.NewGuid());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Resume?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Resume not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var query = new GetResumeByIdQuery(Guid.NewGuid());
        var exception = new Exception("Some error");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting resume by id.");
    }
}