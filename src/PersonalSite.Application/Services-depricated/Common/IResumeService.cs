namespace PersonalSite.Application.Services.Common;

public interface IResumeService : ICrudService<ResumeDto, ResumeAddRequest, ResumeUpdateRequest>
{
    Task<ResumeDto?> GetLatestAsync(CancellationToken cancellationToken = default);
}