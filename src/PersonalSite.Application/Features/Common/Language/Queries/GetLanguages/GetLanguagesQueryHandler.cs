using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguages;

public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, Result<List<LanguageDto>>>
{
    private readonly ILanguageRepository _repository;
    private readonly ILogger<GetLanguagesQueryHandler> _logger;   
    private readonly IMapper<Domain.Entities.Common.Language, LanguageDto> _mapper;   

    public GetLanguagesQueryHandler(
        ILanguageRepository repository,
        ILogger<GetLanguagesQueryHandler> logger,
        IMapper<Domain.Entities.Common.Language, LanguageDto> mapper   
        )
    {
        _repository = repository;
        _logger = logger;   
        _mapper = mapper;  
    }

    public async Task<Result<List<LanguageDto>>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var languages = await _repository.ListAsync(cancellationToken);
            var result = _mapper.MapToDtoList(languages);
            return Result<List<LanguageDto>>.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while retrieving languages.");
            return Result<List<LanguageDto>>.Failure("An unexpected error occurred.");      
        }
    }
}
