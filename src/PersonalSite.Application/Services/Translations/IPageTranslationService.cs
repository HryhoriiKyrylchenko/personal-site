namespace PersonalSite.Application.Services.Translations;

public interface IPageTranslationService : ICrudService<PageTranslationDto, PageTranslationAddRequest, PageTranslationUpdateRequest>
{
    Task<PageTranslationDto?> GetPageByKeyAsync(string pageKey, CancellationToken cancellationToken = default);
}