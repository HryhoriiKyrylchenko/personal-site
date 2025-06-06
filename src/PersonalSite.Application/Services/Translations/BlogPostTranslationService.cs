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
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<BlogPostTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(BlogPostTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(BlogPostTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}