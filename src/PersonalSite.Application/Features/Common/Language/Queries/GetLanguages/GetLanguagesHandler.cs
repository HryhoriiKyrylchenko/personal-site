namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguages;

public class GetLanguagesHandler : IRequestHandler<GetLanguagesQuery, Result<List<LanguageDto>>>
{
    private readonly ILanguageRepository _repository;
    private readonly ILogger<GetLanguagesHandler> _logger;   

    public GetLanguagesHandler(
        ILanguageRepository repository,
        ILogger<GetLanguagesHandler> logger)
    {
        _repository = repository;
        _logger = logger;   
    }

    public async Task<Result<List<LanguageDto>>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var languages = await _repository.ListAsync(cancellationToken);
            var result = EntityToDtoMapper.MapLanguagesToDtoList(languages);
            return Result<List<LanguageDto>>.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while retrieving languages.");
            return Result<List<LanguageDto>>.Failure("An unexpected error occurred.");      
        }
    }
}
