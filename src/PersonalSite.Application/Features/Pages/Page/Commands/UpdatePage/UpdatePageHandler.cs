namespace PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;

public class UpdatePageHandler : IRequestHandler<UpdatePageCommand, Result>
{
    private readonly IPageRepository _pageRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IPageTranslationRepository _translationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePageHandler> _logger;

    public UpdatePageHandler(
        IPageRepository pageRepository, 
        ILanguageRepository languageRepository, 
        IPageTranslationRepository translationRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdatePageHandler> logger)
    {
        _pageRepository = pageRepository;
        _languageRepository = languageRepository;
        _translationRepository = translationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var page = await _pageRepository.GetWithTranslationByIdAsync(request.Id, cancellationToken);
            if (page is null)
            {
                _logger.LogWarning($"Page {request.Id} not found.");
                return Result.Failure("Page not found.");
            }
            
            if (page.Key != request.Key && !await _pageRepository.IsKeyAvailableAsync(request.Key, cancellationToken))
            {
                _logger.LogWarning("A page with this key already exists.");
                return Result.Failure("A page with this key already exists.");
            }

            page.Key = request.Key;
            
            await _pageRepository.UpdateAsync(page, cancellationToken);

            var existingTranslations = await _translationRepository.GetAllByPageKeyAsync(page.Key, 
                cancellationToken);
            
            foreach (var existing in existingTranslations
                         .Where(existing => request.Translations
                             .All(t => t.LanguageCode != existing.Language.Code)))
            {
                _translationRepository.Remove(existing);
            }
            
            foreach (var dto in request.Translations)
            {
                var language = await _languageRepository.GetByCodeAsync(dto.LanguageCode, cancellationToken);
                if (language == null)
                {
                    _logger.LogWarning($"Language {dto.LanguageCode} not found.");
                    return Result.Failure($"Language {dto.LanguageCode} not found.");
                }
            
                var existing = existingTranslations.FirstOrDefault(
                    t => t.LanguageId == language.Id);

                if (existing != null)
                {
                    existing.Data = dto.Data;
                    existing.Title = dto.Title;
                    existing.Description = dto.Description;
                    existing.MetaTitle = dto.MetaTitle;
                    existing.MetaDescription = dto.MetaDescription;
                    existing.OgImage = dto.OgImage;

                    await _translationRepository.UpdateAsync(existing, cancellationToken);
                }
                else
                {
                    var newTranslation = new PageTranslation
                    {
                        Id = Guid.NewGuid(),
                        LanguageId = language.Id,
                        PageId = page.Id,
                        Data = dto.Data,
                        Title = dto.Title,
                        Description = dto.Description,
                        MetaTitle = dto.MetaTitle,
                        MetaDescription = dto.MetaDescription,
                        OgImage = dto.OgImage
                    };

                    await _translationRepository.AddAsync(newTranslation, cancellationToken);
                }
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating {request.Key} page.");
            return Result.Failure($"Error updating {request.Key} page.");      
        }
    }
}