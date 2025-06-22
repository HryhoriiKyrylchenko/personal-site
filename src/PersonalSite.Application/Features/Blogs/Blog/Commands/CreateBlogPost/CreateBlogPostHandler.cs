using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;

public class CreateBlogPostHandler : IRequestHandler<CreateBlogPostCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostTranslationRepository _translationRepository;
    private readonly IBlogPostTagRepository _tagRepository;
    private readonly IPostTagRepository _postTagRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ILogger<CreateBlogPostHandler> _logger;

    public CreateBlogPostHandler(
        IUnitOfWork unitOfWork, 
        IBlogPostRepository blogPostRepository,
        IBlogPostTranslationRepository translationRepository,
        IBlogPostTagRepository tagRepository,
        IPostTagRepository postTagRepository,
        ILanguageRepository languageRepository,
        ILogger<CreateBlogPostHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _blogPostRepository = blogPostRepository;
        _translationRepository = translationRepository;
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _languageRepository = languageRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _blogPostRepository.IsSlugAvailableAsync(request.Slug, cancellationToken))
            {
                _logger.LogWarning($"Blog post slug {request.Slug} is already in use.");
                return Result<Guid>.Failure("Slug is already in use.");
            }

            var blogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Slug = request.Slug,
                CoverImage = request.CoverImage,
                CreatedAt = DateTime.UtcNow,
                IsPublished = request.IsPublished,
                PublishedAt = request.IsPublished ? DateTime.UtcNow : null,
            };

            await _blogPostRepository.AddAsync(blogPost, cancellationToken);

            foreach (var translation in request.Translations)
            {
                var language = await _languageRepository.GetByCodeAsync(translation.LanguageCode, cancellationToken);
                if (language is null)
                {
                    _logger.LogWarning($"Language {translation.LanguageCode} not found.");
                    return Result<Guid>.Failure($"Language {translation.LanguageCode} not found.");
                }

                var newTranslation = new BlogPostTranslation
                {
                    Id = Guid.NewGuid(),
                    LanguageId = language.Id,
                    BlogPostId = blogPost.Id,
                    Title = translation.Title,
                    Excerpt = translation.Excerpt,
                    Content = translation.Content,
                    MetaTitle = translation.MetaTitle,
                    MetaDescription = translation.MetaDescription,
                    OgImage = translation.OgImage
                };
            
                await _translationRepository.AddAsync(newTranslation, cancellationToken);
            }

            foreach (var tag in request.Tags)
            {
                var tagId = tag.Id;
                BlogPostTag? tagEntity;
            
                if (tagId != Guid.Empty)
                {
                    tagEntity = await _tagRepository.GetByIdAsync(tagId, cancellationToken);
                    if (tagEntity == null)
                    {
                        _logger.LogInformation("Tag with ID {TagId} not found. Creating new tag with name {TagName}.", tag.Id, tag.Name);
                    }
                }
                else
                {
                    tagEntity = await _tagRepository.GetByNameAsync(tag.Name, cancellationToken);
                }

                if (tagEntity == null)
                {
                    tagEntity = new BlogPostTag
                    {
                        Id = Guid.NewGuid(),
                        Name = tag.Name
                    };
                    await _tagRepository.AddAsync(tagEntity, cancellationToken);
                }

                tagId = tagEntity.Id;
            
                var postTag = new PostTag
                {
                    Id = Guid.NewGuid(),
                    BlogPostId = blogPost.Id,
                    BlogPostTagId = tagId
                };
                await _postTagRepository.AddAsync(postTag, cancellationToken);
            }
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(blogPost.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating blog post.");
            return Result<Guid>.Failure("Error creating blog post.");
        }
    }
}