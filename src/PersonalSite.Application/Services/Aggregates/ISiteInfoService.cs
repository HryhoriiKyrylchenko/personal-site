namespace PersonalSite.Application.Services.Aggregates;

public interface ISiteInfoService
{
    Task<SiteInfoDto?> GetAsync(CancellationToken cancellationToken = default);
}