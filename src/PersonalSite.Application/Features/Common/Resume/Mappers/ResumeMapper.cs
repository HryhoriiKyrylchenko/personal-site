namespace PersonalSite.Application.Features.Common.Resume.Mappers;

public class ResumeMapper : IMapper<Domain.Entities.Common.Resume, ResumeDto>
{
    private readonly IS3UrlBuilder _urlBuilder;
    
    public ResumeMapper(IS3UrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;   
    }
    
    public ResumeDto MapToDto(Domain.Entities.Common.Resume entity)
    {
        return new ResumeDto
        {
            Id = entity.Id,
            FileUrl = _urlBuilder.BuildUrl(entity.FileUrl),
            FileName = entity.FileName,
            UploadedAt = entity.UploadedAt,
            IsActive = entity.IsActive
        };
    }
    
    public List<ResumeDto> MapToDtoList(IEnumerable<Domain.Entities.Common.Resume> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}