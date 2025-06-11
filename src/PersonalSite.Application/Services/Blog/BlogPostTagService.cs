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
        var newTag = new BlogPostTag
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };
        
        await Repository.AddAsync(newTag, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BlogPostTagUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingTag = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingTag is null) throw new Exception("Tag not found");
        
        existingTag.Name = request.Name;
        
        await Repository.UpdateAsync(existingTag, cancellationToken);
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