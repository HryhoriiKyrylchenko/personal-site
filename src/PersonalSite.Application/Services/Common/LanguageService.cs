namespace PersonalSite.Application.Services.Common;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;
    
    public LanguageService(ILanguageRepository repository) 
    {
        _languageRepository = repository; 
    }

    public async Task<bool> IsSupportedAsync(string code, CancellationToken cancellationToken = default)
    {
        var language = await _languageRepository.GetByCodeAsync(code, cancellationToken);
        return language is not null;
    }
}