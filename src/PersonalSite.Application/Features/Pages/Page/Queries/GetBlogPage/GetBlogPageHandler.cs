namespace PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;

public class GetBlogPageHandler : IRequestHandler<GetBlogPageQuery, Result<BlogPageDto>>
{
    private const string Key = "blog";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ILogger<GetBlogPageHandler> _logger;
    
    public GetBlogPageHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IBlogPostRepository blogPostRepository,
        ILogger<GetBlogPageHandler> logger)
    {
        _language = language;
        _pageRepository = pageRepository;
        _blogPostRepository = blogPostRepository;
        _logger = logger;
    }
    
    public async Task<Result<BlogPageDto>> Handle(GetBlogPageQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_language.LanguageCode))
        {
            return Result<BlogPageDto>.Failure("Invalid language context.");
        }
        
        var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
        if (page == null)
        {
            _logger.LogWarning("Blog page not found.");
            return Result<BlogPageDto>.Failure("Blog page not found.");
        }
        var pageData = EntityToDtoMapper.MapPageToDto(page, _language.LanguageCode);
        
        var blogPosts = await _blogPostRepository.GetAllWithTagsAsync(cancellationToken);
        if (blogPosts.Count < 1)
        {
            _logger.LogWarning("No blog posts found.");     
        }
        var blogPostsData = EntityToDtoMapper.MapBlogPostsToDtoList(blogPosts, _language.LanguageCode);
        
        var blogPage = new BlogPageDto
        {
            PageData = pageData,
            BlogPosts = blogPostsData
        };
        
        return Result<BlogPageDto>.Success(blogPage);
    }
}