namespace PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;

public class GetHomePageHandler : IRequestHandler<GetHomePageQuery, Result<HomePageDto>>
{
    private const string Key = "home";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly IUserSkillRepository _userSkillRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetHomePageHandler> _logger;

    public GetHomePageHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IUserSkillRepository userSkillRepository,
        IProjectRepository projectRepository,
        ILogger<GetHomePageHandler> logger)
    {
        _language = language;
        _pageRepository = pageRepository;
        _userSkillRepository = userSkillRepository;
        _projectRepository = projectRepository;
        _logger = logger;
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
            var pageData = EntityToDtoMapper.MapPageToDto(page, _language.LanguageCode);
        
            var userSkills = await _userSkillRepository.GetAllActiveAsync(cancellationToken);
            if (userSkills.Count < 1)
            {
                _logger.LogWarning("No skills found.");     
            }
            var userSkillsData = EntityToDtoMapper.MapUserSkillsToDtoList(userSkills, _language.LanguageCode);
        
            var lastProject = await _projectRepository.GetLastAsync(cancellationToken);
            if (lastProject == null)
            {
                _logger.LogWarning("No projects found.");   
            }
            var lastProjectData = lastProject == null 
                ? null 
                : EntityToDtoMapper.MapProjectToDto(lastProject, _language.LanguageCode);
        
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