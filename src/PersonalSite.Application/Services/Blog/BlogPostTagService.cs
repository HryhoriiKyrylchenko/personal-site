namespace PersonalSite.Application.Services.Blog;

public class BlogPostTagService : 
    CrudServiceBase<BlogPostTag, BlogPostTagDto, BlogPostTagAddRequest, BlogPostTagUpdateRequest>, 
    IBlogPostTagService
{
    public BlogPostTagService(
        IRepository<BlogPostTag> repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
    }
    
    public override async Task<BlogPostTagDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapBlogPostTagToDto(entity);
    }

    public override async Task<IReadOnlyList<BlogPostTagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        return EntityToDtoMapper.MapBlogPostTagsToDtoList(entities);
    }

    public override async Task AddAsync(BlogPostTagAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(BlogPostTagUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}