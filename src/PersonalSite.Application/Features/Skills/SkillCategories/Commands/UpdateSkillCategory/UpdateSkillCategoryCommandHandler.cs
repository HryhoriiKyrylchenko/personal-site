using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.UpdateSkillCategory;

public class UpdateSkillCategoryCommandHandler : IRequestHandler<UpdateSkillCategoryCommand, Result>
{
    private readonly ISkillCategoryRepository _skillCategoryRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ISkillCategoryTranslationRepository _translationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSkillCategoryCommandHandler> _logger;

    public UpdateSkillCategoryCommandHandler(
        ISkillCategoryRepository skillCategoryRepository,
        ISkillCategoryTranslationRepository translationRepository,
        ILanguageRepository languageRepository,
        ISkillCategoryTranslationRepository skillCategoryTranslationRepository,
        IUnitOfWork unitOfWork, 
        ILogger<UpdateSkillCategoryCommandHandler> logger)
    {
        _skillCategoryRepository = skillCategoryRepository;
        _translationRepository = translationRepository;
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateSkillCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var skillCategory = await _skillCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (skillCategory == null)
            {
                _logger.LogWarning("Skill category not found.");
                return Result.Failure("Skill category not found.");
            }

            if (skillCategory.Key != request.Key && await _skillCategoryRepository.ExistsByKeyAsync(request.Key, cancellationToken))
            {
                _logger.LogWarning("A skill category with this key already exists.");
                return Result.Failure("A skill category with this key already exists.");
            }

            skillCategory.Key = request.Key;
            skillCategory.DisplayOrder = request.DisplayOrder;

            await _skillCategoryRepository.UpdateAsync(skillCategory, cancellationToken);
            
            var existingTranslations = await _translationRepository
                .GetBySkillCategoryIdAsync(skillCategory.Id, cancellationToken);
            
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
                    _logger.LogWarning($"Language '{dto.LanguageCode}' not found.");
                    return Result.Failure($"Language '{dto.LanguageCode}' not found.");
                }
                
                var existing = existingTranslations.FirstOrDefault(t => t.Language.Code == dto.LanguageCode);
                if (existing != null)
                {
                    existing.Name = dto.Name;
                    existing.Description = dto.Description;
                    
                    await _translationRepository.UpdateAsync(existing, cancellationToken);
                }
                else
                {
                    var newTranslation = new SkillCategoryTranslation()
                    {
                        Id = Guid.NewGuid(),
                        LanguageId = language.Id,
                        SkillCategoryId = skillCategory.Id,
                        Name = dto.Name,
                        Description = dto.Description
                    };
                    
                    await _translationRepository.AddAsync(newTranslation, cancellationToken);
                }
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating skill category.");
            return Result.Failure("Error occurred while updating skill category.");
        }
    }
}