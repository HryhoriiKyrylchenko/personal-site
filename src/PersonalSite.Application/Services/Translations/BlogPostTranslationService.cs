namespace PersonalSite.Application.Services.Translations;

public class BlogPostTranslationService : 
    CrudServiceBase<BlogPostTranslation, BlogPostTranslationDto, BlogPostTranslationAddRequest, BlogPostTranslationUpdateRequest>, 
    IBlogPostTranslationService
{
    public BlogPostTranslationService(
        IRepository<BlogPostTranslation> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }

    public override async Task<BlogPostTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var postTranslation = await Repository.GetByIdAsync(id, cancellationToken);
        return postTranslation == null
            ? null
            : EntityToDtoMapper.MapBlogPostTranslationToDto(postTranslation);
    }

    public override async Task<IReadOnlyList<BlogPostTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var postTranslations = await Repository.ListAsync(cancellationToken);
        
        return EntityToDtoMapper.MapBlogPostTranslationsToDtoList(postTranslations);
    }

    public override async Task AddAsync(BlogPostTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
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
        var existingPostTranslation = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingPostTranslation is null) throw new Exception("Blog post translation not found");

        existingPostTranslation.Title = request.Title;
        existingPostTranslation.Excerpt = request.Excerpt;
        existingPostTranslation.Content = request.Content;
        existingPostTranslation.MetaTitle = request.MetaTitle;
        existingPostTranslation.MetaDescription = request.MetaDescription;
        existingPostTranslation.OgImage = request.OgImage;
        
        Repository.Update(existingPostTranslation);
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