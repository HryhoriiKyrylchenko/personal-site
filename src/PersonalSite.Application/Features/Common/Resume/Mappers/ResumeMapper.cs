namespace PersonalSite.Application.Features.Common.Resume.Mappers;

public static class ResumeMapper
{
    public static ResumeDto MapToDto(Domain.Entities.Common.Resume entity)
    {
        return new ResumeDto
        {
            Id = entity.Id,
            FileUrl = S3UrlHelper.BuildImageUrl(entity.FileUrl),
            FileName = entity.FileName,
            UploadedAt = entity.UploadedAt,
            IsActive = entity.IsActive
        };
    }
    
    public static List<ResumeDto> MapToDtoList(IEnumerable<Domain.Entities.Common.Resume> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}