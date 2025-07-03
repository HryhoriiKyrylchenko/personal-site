using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPages;

public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, Result<List<PageAdminDto>>>
{
    private readonly IPageRepository _repository;
    private readonly ILogger<GetPagesQueryHandler> _logger;
    private readonly IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto> _pageMapper;

    public GetPagesQueryHandler(
        IPageRepository repository,
        ILogger<GetPagesQueryHandler> logger,
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