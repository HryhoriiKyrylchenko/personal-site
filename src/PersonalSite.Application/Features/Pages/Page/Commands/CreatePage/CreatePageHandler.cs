namespace PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;

public class CreatePageHandler : IRequestHandler<CreatePageCommand, Result<Guid>>
{
    private readonly IPageRepository _pageRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IPageTranslationRepository _translationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePageHandler> _logger;

    public CreatePageHandler(
        IPageRepository pageRepository, 
        ILanguageRepository languageRepository, 
        IPageTranslationRepository translationRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreatePageHandler> logger)
    {
        _pageRepository = pageRepository;
        _languageRepository = languageRepository;
        _translationRepository = translationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreatePageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _pageRepository.IsKeyAvailableAsync(request.Key, cancellationToken))
            {
                _logger.LogWarning($"Page key {request.Key} is already in use.");
                return Result<Guid>.Failure("Key is already in use.");           
            }
            
            var page = new Domain.Entities.Pages.Page
            {
                Id = Guid.NewGuid(),
                Key = request.Key,
            };
            
            await _pageRepository.AddAsync(page, cancellationToken);

            foreach (var dto in request.Translations)
            {
                var language = await _languageRepository.GetByCodeAsync(dto.LanguageCode, cancellationToken);
                if (language is null)
                {
                    _logger.LogWarning($"Language {dto.LanguageCode} not found.");
                    return Result<Guid>.Failure($"Language {dto.LanguageCode} not found.");
                }

                var translation = new PageTranslation
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

                await _translationRepository.AddAsync(translation, cancellationToken);           
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(page.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating {request.Key} page.");
            return Result<Guid>.Failure($"Error creating {request.Key} page.");       
        }
    }
}
