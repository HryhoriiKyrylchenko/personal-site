using PersonalSite.Application.Services.Aggregates.DTOs;

namespace PersonalSite.Application.Services.Aggregates;

public class PagesDataService : IPagesDataService
{
    private readonly IBlogPostService _blogPostService;
    private readonly IPageService _pageService;
    private readonly IProjectService _projectService;
    private readonly ILearningSkillService _learningSkillService;
    private readonly IUserSkillService _userSkillService;

    public PagesDataService(
        IBlogPostService blogPostService,
        IPageService pageService,
        IProjectService projectService,
        ILearningSkillService learningSkillService,
        IUserSkillService userSkillService)
    {
        _blogPostService = blogPostService;
        _pageService = pageService;
        _projectService = projectService;
        _learningSkillService = learningSkillService;
        _userSkillService = userSkillService;
    }

    public async Task<HomePageDto?> GetHomePageAsync(CancellationToken cancellationToken)
    {
        var pageData = await _pageService.GetByKeyAsync("home", cancellationToken);
        var userSkills = await _userSkillService.GetAllAsync(cancellationToken);
        var lastProject = await _projectService.GetLastProjectAsync(cancellationToken);

        return new HomePageDto
        {
            PageData = pageData,
            UserSkills = userSkills,
            LastProject = lastProject
        };
    }

    public async Task<AboutPageDto?> GetAboutPageAsync(CancellationToken cancellationToken)
    {
        var pageData = await _pageService.GetByKeyAsync("about", cancellationToken);
        var userSkills = await _userSkillService.GetAllAsync(cancellationToken);
        var learningSkills = await _learningSkillService.GetAllAsync(cancellationToken);
        
        return new AboutPageDto
        {
            PageData = pageData,
            UserSkills = userSkills,
            LearningSkills = learningSkills
        };
    }

    public async Task<PortfolioPageDto?> GetPortfolioPageAsync(CancellationToken cancellationToken)
    {
        var pageData = await _pageService.GetByKeyAsync("portfolio", cancellationToken);
        var projects = await _projectService.GetAllAsync(cancellationToken);
        
        return new PortfolioPageDto
        {
            PageData = pageData,
            Projects = projects
        };
    }

    public async Task<BlogPageDto?> GetBlogPageAsync(CancellationToken cancellationToken)
    {
        var pageData = await _pageService.GetByKeyAsync("blog", cancellationToken);
        var posts = await _blogPostService.GetAllAsync(cancellationToken);

        return new BlogPageDto
        {
            PageData = pageData,
            BlogPosts = posts
        };
    }

    public async Task<ContactPageDto?> GetContactPageAsync(CancellationToken cancellationToken)
    {
        var pageData = await _pageService.GetByKeyAsync("contacts", cancellationToken);

        return new ContactPageDto
        {
            PageData = pageData
        };
    }
}