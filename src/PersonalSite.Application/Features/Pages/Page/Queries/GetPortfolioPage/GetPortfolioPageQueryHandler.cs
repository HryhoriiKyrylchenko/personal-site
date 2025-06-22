using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPortfolioPage;

public class GetPortfolioPageQueryHandler : IRequestHandler<GetPortfolioPageQuery, Result<PortfolioPageDto>>
{
    private const string Key = "portfolio";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetPortfolioPageQueryHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    private readonly ITranslatableMapper<Domain.Entities.Projects.Project, ProjectDto> _projectMapper;
    
    public GetPortfolioPageQueryHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IProjectRepository projectRepository,
        ILogger<GetPortfolioPageQueryHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper,
        ITranslatableMapper<Domain.Entities.Projects.Project, ProjectDto> projectMapper)
    {
        _language = language;
        _pageRepository = pageRepository;
        _projectRepository = projectRepository;
        _logger = logger;
        _pageMapper = pageMapper;
        _projectMapper = projectMapper;
    }
    
    public async Task<Result<PortfolioPageDto>> Handle(GetPortfolioPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_language.LanguageCode))
            {
                return Result<PortfolioPageDto>.Failure("Invalid language context.");
            }
        
            var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
            if (page == null)
            {
                _logger.LogWarning("Portfolio page not found.");
                return Result<PortfolioPageDto>.Failure("Portfolio page not found.");
            }
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var projects = await _projectRepository.GetAllWithFullDataAsync(cancellationToken);
            if (projects.Count < 1)
            {
                _logger.LogWarning("No projects found.");     
            }
            var projectsData = _projectMapper.MapToDtoList(projects, _language.LanguageCode);
        
            var portfolioPage = new PortfolioPageDto
            {
                PageData = pageData,
                Projects = projectsData
            };
        
            return Result<PortfolioPageDto>.Success(portfolioPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving portfolio page data.");
            return Result<PortfolioPageDto>.Failure("An unexpected error occurred.");
        }
    }
}