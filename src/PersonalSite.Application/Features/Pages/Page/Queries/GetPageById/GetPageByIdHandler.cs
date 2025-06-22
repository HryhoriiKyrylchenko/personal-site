using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;

public class GetPageByIdHandler : IRequestHandler<GetPageByIdQuery, Result<PageDto>>
{
    private readonly IPageRepository _repository;
    private readonly ILogger<GetPageByIdHandler> _logger;
    private readonly IMapper<Domain.Entities.Pages.Page, PageDto> _mapper;
    
    public GetPageByIdHandler(
        IPageRepository repository,
        ILogger<GetPageByIdHandler> logger,
        IMapper<Domain.Entities.Pages.Page, PageDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<PageDto>> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetWithTranslationByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Page not found.");   
                return Result<PageDto>.Failure("Page not found.");
            }
            var dto = _mapper.MapToDto(entity);
            return Result<PageDto>.Success(dto);       
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting page by id.");
            return Result<PageDto>.Failure("Error getting page by id.");      
        }
    }
}