namespace PersonalSite.Application.Services.Blog;

public interface IBlogPostService : ICrudService<BlogPostDto, BlogPostAddRequest, BlogPostUpdateRequest>
{
    Task PublishPostAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogPostDto>> GetPublishedPostsAsync(CancellationToken cancellationToken = default);
    Task AssignTagsToPost(Guid postId, IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlogPostTagDto>> GetTagsForPost(Guid postId, CancellationToken cancellationToken = default);
    Task RemoveTagsFromPost(Guid postId, IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default);
}