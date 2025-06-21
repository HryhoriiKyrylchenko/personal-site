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
        return await _languageRepository.ExistsByCodeAsync(code, cancellationToken);
    }
}