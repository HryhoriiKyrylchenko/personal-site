namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;

public class CreateSkillCategoryHandler : IRequestHandler<CreateSkillCategoryCommand, Result<Guid>>
{
    private readonly ISkillCategoryRepository _skillCategoryRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ISkillCategoryTranslationRepository _translationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSkillCategoryHandler> _logger;

    public CreateSkillCategoryHandler(
        ISkillCategoryRepository skillCategoryRepository,
        ISkillCategoryTranslationRepository translationRepository,
        ILanguageRepository languageRepository,
        ISkillCategoryTranslationRepository skillCategoryTranslationRepository,
        IUnitOfWork unitOfWork, 
        ILogger<CreateSkillCategoryHandler> logger)
    {
        _skillCategoryRepository = skillCategoryRepository;
        _translationRepository = translationRepository;
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateSkillCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _skillCategoryRepository.ExistsByKeyAsync(request.Key, cancellationToken))
            {
                _logger.LogWarning($"A skill category with key {request.Key} already exists.");
                return Result<Guid>.Failure($"A skill category with key {request.Key} already exists.");
            }

            var skillCategory = new SkillCategory
            {
                Id = Guid.NewGuid(),
                Key = request.Key,
                DisplayOrder = request.DisplayOrder
            };

            await _skillCategoryRepository.AddAsync(skillCategory, cancellationToken);

            foreach (var translation in request.Translations)
            {
                var language = await _languageRepository.GetByCodeAsync(translation.LanguageCode, cancellationToken);
                if (language is null)
                {
                    _logger.LogWarning($"Language {translation.LanguageCode} not found.");
                    return Result<Guid>.Failure($"Language {translation.LanguageCode} not found.");
                }
                
                var skillCategoryTranslation = new SkillCategoryTranslation
                {
                    Id = Guid.NewGuid(),
                    LanguageId = language.Id,
                    SkillCategoryId = skillCategory.Id,
                    Name = translation.Name,
                    Description = translation.Description
                };
                
                await _translationRepository.AddAsync(skillCategoryTranslation, cancellationToken);
            }
            
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(skillCategory.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill category.");
            return Result<Guid>.Failure("Error occurred while creating skill category.");
        }
    }
}