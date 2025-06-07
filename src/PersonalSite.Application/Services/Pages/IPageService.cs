namespace PersonalSite.Application.Services.Pages;

public interface IPageService : ICrudService<PageDto, PageAddRequest, PageUpdateRequest>
{
    Task<PageDto?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
}