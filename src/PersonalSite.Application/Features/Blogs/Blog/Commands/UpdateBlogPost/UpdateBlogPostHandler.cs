using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;

public class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPostCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostTranslationRepository _translationRepository;
    private readonly IBlogPostTagRepository _tagRepository;
    private readonly IPostTagRepository _postTagRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly ILogger<UpdateBlogPostHandler> _logger;

    public UpdateBlogPostHandler(
        IUnitOfWork unitOfWork, 
        IBlogPostRepository blogPostRepository,
        IBlogPostTranslationRepository translationRepository,
        IBlogPostTagRepository tagRepository,
        IPostTagRepository postTagRepository,
        ILanguageRepository languageRepository,
        ILogger<UpdateBlogPostHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _blogPostRepository = blogPostRepository;
        _translationRepository = translationRepository;
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _languageRepository = languageRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(request.Id, cancellationToken);

            if (blogPost == null)
            {
                _logger.LogWarning("Blog post with ID {Id} not found.", request.Id);
                return Result.Failure("Blog post not found.");
            }
            
            if (blogPost.Slug != request.Slug && !await _blogPostRepository.IsSlugAvailableAsync(request.Slug, cancellationToken))
            {
                _logger.LogWarning("A skill with this key already exists.");
                return Result.Failure("A skill with this key already exists.");
            }

            blogPost.Slug = request.Slug;
            blogPost.CoverImage = request.CoverImage;
            blogPost.IsDeleted = request.IsDeleted;
            blogPost.UpdatedAt = DateTime.UtcNow;
        
            await _blogPostRepository.UpdateAsync(blogPost, cancellationToken);

            var existingTranslations = await _translationRepository.GetByBlogPostIdAsync(blogPost.Id, cancellationToken);

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
                    return Result.Failure($"Language {dto.LanguageCode} not found.");
                }
            
                var existing = existingTranslations
                    .FirstOrDefault(t => t.LanguageId == language.Id);

                if (existing != null)
                {
                    existing.Title = dto.Title;
                    existing.Excerpt = dto.Excerpt;
                    existing.Content = dto.Content;
                    existing.MetaTitle = dto.MetaTitle;
                    existing.MetaDescription = dto.MetaDescription;
                    existing.OgImage = dto.OgImage;

                    await _translationRepository.UpdateAsync(existing, cancellationToken);
                }
                else
                {
                    var newTranslation = new BlogPostTranslation
                    {
                        Id = Guid.NewGuid(),
                        LanguageId = language.Id,
                        BlogPostId = blogPost.Id,
                        Title = dto.Title,
                        Excerpt = dto.Excerpt,
                        Content = dto.Content,
                        MetaTitle = dto.MetaTitle,
                        MetaDescription = dto.MetaDescription,
                        OgImage = dto.OgImage
                    };

                    await _translationRepository.AddAsync(newTranslation, cancellationToken);
                }
            }
        
            var existingPostTags = await _postTagRepository.GetByBlogPostIdAsync(blogPost.Id, cancellationToken);

            foreach (var existing in existingPostTags
                         .Where(existing => request.Tags
                             .All(t => t.Id != existing.BlogPostTagId)))
            {
                _postTagRepository.Remove(existing);
            }

            foreach (var tag in request.Tags)
            {
                if (existingPostTags.Any(t => t.BlogPostTagId == tag.Id)) 
                    continue;
            
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

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating blog post.");
            return Result.Failure("Error updating blog post.");       
        }
    }
}