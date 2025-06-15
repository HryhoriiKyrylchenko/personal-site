namespace PersonalSite.Application.Services.Blog;

public class BlogPostService : 
    CrudServiceBase<BlogPost, BlogPostDto, BlogPostAddRequest, BlogPostUpdateRequest>, 
    IBlogPostService
{
    private readonly LanguageContext _language;
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IPostTagRepository _postTagRepository;
    
    public BlogPostService(
        IBlogPostRepository blogPostRepository,
        IPostTagRepository postTagRepository,
        IUnitOfWork unitOfWork, 
        LanguageContext language,
        ILogger<CrudServiceBase<BlogPost, BlogPostDto, BlogPostAddRequest, BlogPostUpdateRequest>> logger,
        IServiceProvider serviceProvider)
        : base(blogPostRepository, unitOfWork, logger, serviceProvider)
    {
        _language = language;
        _blogPostRepository = blogPostRepository;
        _postTagRepository = postTagRepository;
    }
    
    public override async Task<BlogPostDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _blogPostRepository.GetByIdWithTagsAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapBlogPostToDto(entity, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<BlogPostDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _blogPostRepository.GetAllWithTagsAsync(cancellationToken);
        return EntityToDtoMapper.MapBlogPostsToDtoList(entities, _language.LanguageCode);
    }

    public override async Task AddAsync(BlogPostAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newPost = new BlogPost
        {
            Id = Guid.NewGuid(),
            Slug = request.Slug,
            CoverImage = request.CoverImage,
            CreatedAt = DateTime.UtcNow,
            IsPublished = false
        };
        
        await _blogPostRepository.AddAsync(newPost, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BlogPostUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingPost = await _blogPostRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingPost is null) throw new Exception("Post not found");
        
        existingPost.Slug = request.Slug;
        existingPost.CoverImage = request.CoverImage;
        existingPost.UpdatedAt = DateTime.UtcNow;
        
        await _blogPostRepository.UpdateAsync(existingPost, cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _blogPostRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _blogPostRepository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task PublishPostAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await Repository.GetByIdAsync(id, cancellationToken);
        if (post is null) throw new Exception("Post not found");

        post.IsPublished = true;
        post.PublishedAt = DateTime.UtcNow;

        await Repository.UpdateAsync(post, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<BlogPostDto>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        var posts = await _blogPostRepository.GetPublishedPostsAsync(cancellationToken);
        return EntityToDtoMapper.MapBlogPostsToDtoList(posts, _language.LanguageCode);
    }

    public async Task AssignTagsToPost(Guid postId, IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default)
    {
        var post = await _blogPostRepository.GetByIdWithTagsAsync(postId, cancellationToken);
        if (post is null) throw new Exception("Post not found");

        foreach (var tagId in tagIds)
        {
            if (await _postTagRepository.ExistsAsync(e => e.Id == tagId, cancellationToken)) continue;

            if (post.PostTags.All(pt => pt.Id != tagId))
            {
                await _postTagRepository.AddAsync(new PostTag
                {
                    Id = Guid.NewGuid(),
                    BlogPostId = postId,
                    BlogPostTagId = tagId
                }, cancellationToken);
            }
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BlogPostTagDto>> GetTagsForPost(Guid postId, CancellationToken cancellationToken = default)
    {
        var post = await _blogPostRepository.GetByIdWithTagsAsync(postId, cancellationToken);
        if (post is null) throw new Exception("Post not found");

        return EntityToDtoMapper.MapBlogPostTagsToDtoList(post.PostTags.Select(pt => pt.BlogPostTag));
    }

    public async Task RemoveTagsFromPost(Guid postId, IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default)
    {
        var post = await _blogPostRepository.GetByIdWithTagsAsync(postId, cancellationToken);
        if (post is null) throw new Exception("Post not found");

        foreach (var tagId in tagIds)
        {
            var tag = post.PostTags.FirstOrDefault(t => t.Id == tagId);
            if (tag != null)
            {
                _postTagRepository.Remove(tag);
            }
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}