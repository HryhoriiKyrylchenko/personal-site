using PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Commands.DeleteBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Commands.PublishBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPosts;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Blog;

[Route("api/admin/[controller]")]
[ApiController]
//[Authorize]
public class BlogPostController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlogPostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result>> Update([FromRoute] Guid id, [FromBody] UpdateBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched BlogPost ID.");

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBlogPostCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<ActionResult<Result>> Publish([FromRoute] Guid id, [FromBody] PublishBlogPostCommand command, 
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched BlogPost ID.");
        
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return Ok("Blog post published successfully.");
        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BlogPostAdminDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBlogPostByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<BlogPostAdminDto>>> GetAll([FromQuery] GetBlogPostsQuery query, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result);
    }
}
