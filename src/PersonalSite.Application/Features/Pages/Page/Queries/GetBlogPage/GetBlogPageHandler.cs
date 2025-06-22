namespace PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;

public class GetBlogPageHandler : IRequestHandler<GetBlogPageQuery, Result<BlogPageDto>>
{
    private const string Key = "blog";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ILogger<GetBlogPageHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    private readonly ITranslatableMapper<BlogPost, BlogPostDto> _blogPostMapper;
    
    public GetBlogPageHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IBlogPostRepository blogPostRepository,
        ILogger<GetBlogPageHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper,
        ITranslatableMapper<BlogPost, BlogPostDto> blogPostMapper)
    {
        _language = language;
        _pageRepository = pageRepository;
        _blogPostRepository = blogPostRepository;
        _logger = logger;
        _pageMapper = pageMapper;
        _blogPostMapper = blogPostMapper; 
    }
    
    public async Task<Result<BlogPageDto>> Handle(GetBlogPageQuery request, CancellationToken cancellationToken)
    {
        try
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
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var blogPosts = await _blogPostRepository.GetPublishedPostsAsync(cancellationToken);
            if (blogPosts.Count < 1)
            {
                _logger.LogWarning("No blog posts found.");     
            }
            var blogPostsData = _blogPostMapper.MapToDtoList(blogPosts, _language.LanguageCode);
        
            var blogPage = new BlogPageDto
            {
                PageData = pageData,
                BlogPosts = blogPostsData
            };
        
            return Result<BlogPageDto>.Success(blogPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving blog page data.");
            return Result<BlogPageDto>.Failure("An unexpected error occurred.");
        }
    }
}