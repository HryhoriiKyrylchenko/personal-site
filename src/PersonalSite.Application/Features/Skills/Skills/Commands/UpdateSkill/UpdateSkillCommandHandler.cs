using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Features.Skills.Skills.Commands.UpdateSkill;

public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, Result>
{
    private readonly ISkillRepository _skillRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ISkillTranslationRepository _translationRepository;
    private readonly ISkillCategoryRepository _skillCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSkillCommandHandler> _logger;

    public UpdateSkillCommandHandler(
        ISkillRepository skillRepository, 
        ISkillCategoryRepository skillCategoryRepository, 
        ISkillTranslationRepository translationRepository,
        ILanguageRepository languageRepository,
        IUnitOfWork unitOfWork, 
        ILogger<UpdateSkillCommandHandler> logger)
    {
        _skillRepository = skillRepository;
        _skillCategoryRepository = skillCategoryRepository;
        _translationRepository = translationRepository;
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var skill = await _skillRepository.GetByIdAsync(request.Id, cancellationToken);
            if (skill == null)
            {
                _logger.LogWarning("Skill not found.");
                return Result.Failure("Skill not found.");
            }
            
            if (skill.Key != request.Key && await _skillRepository.ExistsByKeyAsync(request.Key, cancellationToken))
            {
                _logger.LogWarning("A skill with this key already exists.");
                return Result.Failure("A skill with this key already exists.");
            }

            skill.Key = request.Key;
            skill.CategoryId = request.CategoryId;

            await _skillRepository.UpdateAsync(skill, cancellationToken);
            
            var existingTranslations = await _translationRepository.GetBySkillIdAsync(skill.Id, cancellationToken);
            
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
                    var newTranslation = new SkillTranslation
                    {
                        Id = Guid.NewGuid(),
                        LanguageId = language.Id,
                        SkillId = skill.Id,
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
            _logger.LogError(ex, "Error while updating skill.");
            return Result.Failure("Error while updating skill.");
        }
    }
}
