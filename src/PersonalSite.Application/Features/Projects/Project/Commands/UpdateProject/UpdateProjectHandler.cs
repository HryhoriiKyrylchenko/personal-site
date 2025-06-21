namespace PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IProjectTranslationRepository _translationRepository;
    private readonly IProjectSkillRepository _projectSkillRepository;
    private readonly ISkillRepository _skillRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProjectHandler> _logger;

    public UpdateProjectHandler(
        IProjectRepository projectRepository,
        ILanguageRepository languageRepository,
        IProjectTranslationRepository translationRepository,
        IProjectSkillRepository projectSkillRepository,
        ISkillRepository skillRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateProjectHandler> logger)
    {
        _projectRepository = projectRepository;
        _languageRepository = languageRepository;
        _translationRepository = translationRepository;
        _projectSkillRepository = projectSkillRepository;
        _skillRepository = skillRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectRepository.GetWithFullDataAsync(request.Id, cancellationToken);
            if (project == null)
            {
                _logger.LogWarning($"Project with ID {request.Id} not found.");
                return Result.Failure($"Project with ID {request.Id} not found.");
            }
            
            if (project.Slug != request.Slug && !await _projectRepository.IsSlugAvailableAsync(request.Slug, cancellationToken))
            {
                _logger.LogWarning("A project with this slug already exists.");
                return Result.Failure("A project with this slug already exists.");
            }

            project.Slug = request.Slug;
            project.CoverImage = request.CoverImage;
            project.DemoUrl = request.DemoUrl;
            project.RepoUrl = request.RepoUrl;
            project.UpdatedAt = DateTime.UtcNow;
            
            await _projectRepository.UpdateAsync(project, cancellationToken);
            
            var existingTranslations = await _translationRepository.GetByProjectIdAsync(project.Id, cancellationToken);

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
                    return Result.Failure($"Language '{dto.LanguageCode}' not found.");
                }

                var existing = existingTranslations
                    .FirstOrDefault(t => t.LanguageId == language.Id);
                
                if (existing != null)
                {
                    existing.Title = dto.Title;
                    existing.ShortDescription = dto.ShortDescription;
                    existing.DescriptionSections = dto.DescriptionSections;
                    existing.MetaTitle = dto.MetaTitle;
                    existing.MetaDescription = dto.MetaDescription;
                    existing.OgImage = dto.OgImage;
                    
                    await _translationRepository.UpdateAsync(existing, cancellationToken);
                }
                else
                {
                    var translation = new ProjectTranslation
                    {
                        Id = Guid.NewGuid(),
                        LanguageId = language.Id,
                        ProjectId = project.Id,
                        Title = dto.Title,
                        ShortDescription = dto.ShortDescription,
                        DescriptionSections = dto.DescriptionSections,
                        MetaTitle = dto.MetaTitle,
                        MetaDescription = dto.MetaDescription,
                        OgImage = dto.OgImage
                    };
                    
                    await _translationRepository.AddAsync(translation, cancellationToken);
                }
            }
            
            var existingSkills = await _projectSkillRepository.GetByProjectIdAsync(project.Id, cancellationToken);
            foreach (var skill in existingSkills.Where(skill => !request.SkillIds.Contains(skill.Id)))
            {
                _projectSkillRepository.Remove(skill);
            }

            foreach (var skillId in request.SkillIds)
            {
                if (existingSkills.Any(ps => ps.SkillId == skillId)) 
                    continue;

                var skill = await _skillRepository.GetByIdAsync(skillId, cancellationToken);
                if (skill == null)
                {
                    _logger.LogWarning($"Skill with ID {skillId} not found.");
                    return Result.Failure($"Skill with ID {skillId} not found.");
                }

                var newProjectSkill = new ProjectSkill
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project.Id,
                    SkillId = skillId
                };
                
                await _projectSkillRepository.AddAsync(newProjectSkill, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating project.");
            return Result.Failure("Error updating project.");       
        }
    }
}