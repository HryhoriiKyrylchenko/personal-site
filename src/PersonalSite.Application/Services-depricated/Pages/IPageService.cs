using PersonalSite.Application.Features.Pages.Common.Dtos;

namespace PersonalSite.Application.Services.Pages;

public interface IPageService : ICrudService<PageDto, PageAddRequest, PageUpdateRequest>
{
    Task<PageDto?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
}