namespace PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;

public class CreateSkillHandler : IRequestHandler<CreateSkillCommand, Result<Guid>>
{
    private readonly ISkillRepository _skillRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ISkillTranslationRepository _translationRepository;
    private readonly ISkillCategoryRepository _skillCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSkillHandler> _logger;

    public CreateSkillHandler(
        ISkillRepository skillRepository, 
        ISkillCategoryRepository skillCategoryRepository, 
        ISkillTranslationRepository translationRepository,
        ILanguageRepository languageRepository,
        IUnitOfWork unitOfWork, 
        ILogger<CreateSkillHandler> logger)
    {
        _skillRepository = skillRepository;
        _skillCategoryRepository = skillCategoryRepository;
        _translationRepository = translationRepository;
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _skillRepository.ExistsByKeyAsync(request.Key, cancellationToken);
            if (exists)
            {
                _logger.LogWarning("Skill key already exists."); 
                return Result<Guid>.Failure("Skill key already exists.");
            }
            
            var skillCategory = await _skillCategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (skillCategory == null)
            {
                _logger.LogWarning($"Skill category with ID {request.CategoryId} not found.");
                return Result<Guid>.Failure($"Skill category with ID {request.CategoryId} not found.");
            }

            var skill = new Skill
            {
                Id = Guid.NewGuid(),
                CategoryId = request.CategoryId,
                Key = request.Key
            };

            await _skillRepository.AddAsync(skill, cancellationToken);

            foreach (var dto in request.Translations)
            {
                var language = await _languageRepository.GetByCodeAsync(dto.LanguageCode, cancellationToken);
                if (language == null)
                {
                    _logger.LogWarning($"Language '{dto.LanguageCode}' not found.");
                    return Result<Guid>.Failure($"Language '{dto.LanguageCode}' not found.");
                }
                
                var translation = new SkillTranslation
                {
                    Id = Guid.NewGuid(),
                    LanguageId = language.Id,
                    SkillId = skill.Id,
                    Name = dto.Name,
                    Description = dto.Description
                };
                
                await _translationRepository.AddAsync(translation, cancellationToken);
            }    
                
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(skill.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating skill.");
            return Result<Guid>.Failure("Error while creating skill.");
        }
    }
}