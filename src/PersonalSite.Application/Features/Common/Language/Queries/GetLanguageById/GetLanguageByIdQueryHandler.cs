using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguageById;

public class GetLanguageByIdQueryHandler : IRequestHandler<GetLanguageByIdQuery, Result<LanguageDto>>
{
    private readonly ILanguageRepository _repository;
    private readonly ILogger<GetLanguageByIdQueryHandler> _logger;   
    private readonly IMapper<Domain.Entities.Common.Language, LanguageDto> _mapper;   
    
    public GetLanguageByIdQueryHandler(
        ILanguageRepository repository,
        ILogger<GetLanguageByIdQueryHandler> logger,
        IMapper<Domain.Entities.Common.Language, LanguageDto> mapper)
    {
        _repository = repository;
        _logger = logger;   
        _mapper = mapper;
    }


    public async Task<Result<LanguageDto>> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var language = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (language == null)
            {
                _logger.LogWarning("Language with ID {Id} not found.", request.Id);
                return Result<LanguageDto>.Failure("Language not found.");
            }
            var result = _mapper.MapToDto(language);
            return Result<LanguageDto>.Success(result);       
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting language by id.");
            return Result<LanguageDto>.Failure("Error getting language by id.");      
        }
    }
}