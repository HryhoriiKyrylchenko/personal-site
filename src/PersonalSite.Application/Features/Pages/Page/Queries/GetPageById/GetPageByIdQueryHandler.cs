using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;

public class GetPageByIdQueryHandler : IRequestHandler<GetPageByIdQuery, Result<PageAdminDto>>
{
    private readonly IPageRepository _repository;
    private readonly ILogger<GetPageByIdQueryHandler> _logger;
    private readonly IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto> _mapper;
    
    public GetPageByIdQueryHandler(
        IPageRepository repository,
        ILogger<GetPageByIdQueryHandler> logger,
        IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<Result<PageAdminDto>> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetWithTranslationByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Page not found.");   
                return Result<PageAdminDto>.Failure("Page not found.");
            }
            var dto = _mapper.MapToAdminDto(entity);
            return Result<PageAdminDto>.Success(dto);       
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting page by id.");
            return Result<PageAdminDto>.Failure("Error getting page by id.");      
        }
    }
}