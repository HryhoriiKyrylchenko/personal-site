using PersonalSite.Application.Features.Common.Resume.Mappers;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Common.Resume;

public class ResumeMapperTests
    {
        private readonly Mock<IS3UrlBuilder> _urlBuilderMock;
        private readonly ResumeMapper _mapper;

        public ResumeMapperTests()
        {
            _urlBuilderMock = new Mock<IS3UrlBuilder>();
            _mapper = new ResumeMapper(_urlBuilderMock.Object);
        }

        [Fact]
        public void MapToDto_ShouldMapAllProperties_AndBuildUrl()
        {
            // Arrange
            var resume = CommonTestDataFactory.CreateResume(
                fileName: "resume.pdf",
                fileUrl: "file/path/resume.pdf",
                isActive: true);

            var expectedUrl = "https://bucket.s3.amazonaws.com/file/path/resume.pdf";
            _urlBuilderMock.Setup(x => x.BuildUrl(resume.FileUrl)).Returns(expectedUrl);

            // Act
            var dto = _mapper.MapToDto(resume);

            // Assert
            dto.Id.Should().Be(resume.Id);
            dto.FileUrl.Should().Be(expectedUrl);
            dto.FileName.Should().Be(resume.FileName);
            dto.UploadedAt.Should().Be(resume.UploadedAt);
            dto.IsActive.Should().Be(resume.IsActive);

            _urlBuilderMock.Verify(x => x.BuildUrl(resume.FileUrl), Times.Once);
        }

        [Fact]
        public void MapToDtoList_ShouldMapAllEntities()
        {
            // Arrange
            var resumes = new List<Domain.Entities.Common.Resume>
            {
                CommonTestDataFactory.CreateResume(
                    fileUrl: "file1.pdf",
                    fileName : "file1.pdf",
                    isActive : true
                    ),
                CommonTestDataFactory.CreateResume(
                    fileUrl: "file2.pdf",
                    fileName : "file2.pdf",
                    isActive : false)
            };

            _urlBuilderMock.Setup(x => x.BuildUrl(It.IsAny<string>()))
                .Returns<string>(url => "https://bucket.s3.amazonaws.com/" + url);

            // Act
            var dtos = _mapper.MapToDtoList(resumes);

            // Assert
            dtos.Should().HaveCount(resumes.Count);

            for (int i = 0; i < resumes.Count; i++)
            {
                dtos[i].Id.Should().Be(resumes[i].Id);
                dtos[i].FileUrl.Should().Be("https://bucket.s3.amazonaws.com/" + resumes[i].FileUrl);
                dtos[i].FileName.Should().Be(resumes[i].FileName);
                dtos[i].UploadedAt.Should().Be(resumes[i].UploadedAt);
                dtos[i].IsActive.Should().Be(resumes[i].IsActive);
            }

            _urlBuilderMock.Verify(x => x.BuildUrl(It.IsAny<string>()), Times.Exactly(resumes.Count));
        }
    }