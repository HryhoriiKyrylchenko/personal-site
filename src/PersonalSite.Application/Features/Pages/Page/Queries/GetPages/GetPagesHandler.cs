using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPages;

public class GetPagesHandler : IRequestHandler<GetPagesQuery, Result<List<PageAdminDto>>>
{
    private readonly IPageRepository _repository;
    private readonly ILogger<GetPagesHandler> _logger;
    private readonly IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto> _pageMapper;

    public GetPagesHandler(
        IPageRepository repository,
        ILogger<GetPagesHandler> logger,
        IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto> pageMapper)
    {
        _repository = repository;
        _logger = logger;
        _pageMapper = pageMapper;
    }

    public async Task<Result<List<PageAdminDto>>> Handle(GetPagesQuery request, CancellationToken cancellationToken)
    {
        var pages = await _repository.GetAllWithTranslationsAsync(cancellationToken);

        if (pages.Count == 0)
        {
            _logger.LogWarning("No pages found.");
            return Result<List<PageAdminDto>>.Failure("No pages found.");
        }

        return Result<List<PageAdminDto>>.Success(_pageMapper.MapToAdminDtoList(pages));
    }
}