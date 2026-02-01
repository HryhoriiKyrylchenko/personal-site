using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IProjectTranslationRepository _translationRepository;
    private readonly IProjectSkillRepository _projectSkillRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProjectCommandHandler> _logger;
    private readonly IS3UrlBuilder _urlBuilder;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        ILanguageRepository languageRepository,
        IProjectTranslationRepository translationRepository,
        IProjectSkillRepository projectSkillRepository,
        ISkillRepository skillRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateProjectCommandHandler> logger,
        IS3UrlBuilder urlBuilder)
    {
        _projectRepository = projectRepository;
        _languageRepository = languageRepository;
        _translationRepository = translationRepository;
        _projectSkillRepository = projectSkillRepository;
        _skillRepository = skillRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _urlBuilder = urlBuilder;
    }

    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _projectRepository.IsSlugAvailableAsync(request.Slug, cancellationToken))
            {
                _logger.LogWarning($"Blog post slug {request.Slug} is already in use.");
                return Result<Guid>.Failure("Slug is already in use.");
            }

            var project = new Domain.Entities.Projects.Project
            {
                Id = Guid.NewGuid(),
                Slug = request.Slug,
                CoverImage = string.IsNullOrWhiteSpace(request.CoverImage)
                    ? string.Empty
                    : _urlBuilder.ExtractKey(request.CoverImage),
                DemoUrl = request.DemoUrl,
                RepoUrl = request.RepoUrl,
                CreatedAt = DateTime.UtcNow
            };
        
            await _projectRepository.AddAsync(project, cancellationToken);

            foreach (var dto in request.Translations)
            {
                var language = await _languageRepository.GetByCodeAsync(dto.LanguageCode, cancellationToken);
                if (language == null)
                {
                    _logger.LogWarning($"Language '{dto.LanguageCode}' not found.");
                    continue;
                }

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
                    OgImage = string.IsNullOrWhiteSpace(request.CoverImage)
                        ? string.Empty
                        : _urlBuilder.ExtractKey(dto.OgImage)
                };

                await _translationRepository.AddAsync(translation, cancellationToken);
            }

            foreach (var skillId in request.SkillIds)
            {
                var skill = await _skillRepository.GetByIdAsync(skillId, cancellationToken);
                if (skill == null)
                {
                    _logger.LogWarning($"Skill with ID {skillId} not found.");
                    continue;
                }

                var projectSkill = new ProjectSkill
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project.Id,
                    SkillId = skillId
                };
            
                await _projectSkillRepository.AddAsync(projectSkill, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(project.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating project.");
            return Result<Guid>.Failure("Error creating project.");
        }
    }
}