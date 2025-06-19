using PersonalSite.Application.Features.Translations.Common.Dtos;

namespace PersonalSite.Application.Services.Translations;

public class BlogPostTranslationService : 
    CrudServiceBase<BlogPostTranslation, BlogPostTranslationDto, BlogPostTranslationAddRequest, BlogPostTranslationUpdateRequest>, 
    IBlogPostTranslationService
{
    private IBlogPostTranslationRepository _blogPostTranslationRepository;
    
    public BlogPostTranslationService(
        IBlogPostTranslationRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CrudServiceBase<BlogPostTranslation, BlogPostTranslationDto, BlogPostTranslationAddRequest, BlogPostTranslationUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _blogPostTranslationRepository = repository;   
    }

    public override async Task<BlogPostTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var postTranslation = await _blogPostTranslationRepository.GetWithLanguageByIdAsync(id, cancellationToken);
        return postTranslation == null
            ? null
            : EntityToDtoMapper.MapBlogPostTranslationToDto(postTranslation);
    }

    public override async Task<IReadOnlyList<BlogPostTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var postTranslations = await _blogPostTranslationRepository.ListWithLanguageAsync(cancellationToken);
        
        return EntityToDtoMapper.MapBlogPostTranslationsToDtoList(postTranslations);
    }

    public override async Task AddAsync(BlogPostTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newPostTranslation = new BlogPostTranslation
        {
            Id = Guid.NewGuid(),
            LanguageId = request.LanguageId,
            BlogPostId = request.BlogPostId,
            Title = request.Title,
            Excerpt = request.Excerpt,
            Content = request.Content,
            MetaTitle = request.MetaTitle,
            MetaDescription = request.MetaDescription,
            OgImage = request.OgImage
        };
        
        await Repository.AddAsync(newPostTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BlogPostTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingPostTranslation = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingPostTranslation is null) throw new Exception("Blog post translation not found");

        existingPostTranslation.Title = request.Title;
        existingPostTranslation.Excerpt = request.Excerpt;
        existingPostTranslation.Content = request.Content;
        existingPostTranslation.MetaTitle = request.MetaTitle;
        existingPostTranslation.MetaDescription = request.MetaDescription;
        existingPostTranslation.OgImage = request.OgImage;
        
        await Repository.UpdateAsync(existingPostTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            Repository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }   
    }
}