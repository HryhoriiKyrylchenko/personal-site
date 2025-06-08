namespace PersonalSite.Application.Services.Translations;

public interface IPageTranslationService : ICrudService<PageTranslationDto, PageTranslationAddRequest, PageTranslationUpdateRequest>
{
    Task<List<PageTranslationDto>> GetAllByPageKeyAsync(string pageKey, CancellationToken cancellationToken = default);
}