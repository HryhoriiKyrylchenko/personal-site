using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Projects;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;

public class GetHomePageQueryHandler : IRequestHandler<GetHomePageQuery, Result<HomePageDto>>
{
    private const string Key = "home";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly IUserSkillRepository _userSkillRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetHomePageQueryHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    private readonly ITranslatableMapper<UserSkill, UserSkillDto> _userSkillMapper;
    private readonly ITranslatableMapper<Project, ProjectDto> _projectMapper;

    public GetHomePageQueryHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IUserSkillRepository userSkillRepository,
        IProjectRepository projectRepository,
        ILogger<GetHomePageQueryHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper,
        ITranslatableMapper<UserSkill, UserSkillDto> userSkillMapper,
        ITranslatableMapper<Project, ProjectDto> projectMapper)
    {
        _language = language;
        _pageRepository = pageRepository;
        _userSkillRepository = userSkillRepository;
        _projectRepository = projectRepository;
        _logger = logger;
        _pageMapper = pageMapper;
        _userSkillMapper = userSkillMapper;
        _projectMapper = projectMapper;
    }
    
    public async Task<Result<HomePageDto>> Handle(GetHomePageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_language.LanguageCode))
            {
                return Result<HomePageDto>.Failure("Invalid language context.");
            }
        
            var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
            if (page == null)
            {
                _logger.LogWarning("About page not found.");
                return Result<HomePageDto>.Failure("About page not found.");
            }
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var userSkills = await _userSkillRepository.GetAllActiveAsync(cancellationToken);
            if (userSkills.Count < 1)
            {
                _logger.LogWarning("No skills found.");     
            }
            var userSkillsData = _userSkillMapper.MapToDtoList(userSkills, _language.LanguageCode);
        
            var lastProject = await _projectRepository.GetLastAsync(cancellationToken);
            if (lastProject == null)
            {
                _logger.LogWarning("No projects found.");   
            }
            var lastProjectData = lastProject == null 
                ? null 
                : _projectMapper.MapToDto(lastProject, _language.LanguageCode);
        
            var aboutPage = new HomePageDto
            {
                PageData = pageData,
                UserSkills = userSkillsData,
                LastProject = lastProjectData
            };
        
            return Result<HomePageDto>.Success(aboutPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving home page data.");
            return Result<HomePageDto>.Failure("An unexpected error occurred.");
        }
    }
}