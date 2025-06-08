using PersonalSite.Application.Services.Translations.DTOs;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/localization")]
public class LocalizationController : ControllerBase
{
    private readonly ILanguageService _languageService;
    
    public LocalizationController(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    [HttpGet("languages")]
    public async Task<ActionResult<IReadOnlyList<LanguageDto>>> GetSupportedLanguagesAsync(
        CancellationToken cancellationToken = default)
    {
        var languages = await _languageService.GetAllAsync(cancellationToken);
        
        return Ok(languages);
    }
}