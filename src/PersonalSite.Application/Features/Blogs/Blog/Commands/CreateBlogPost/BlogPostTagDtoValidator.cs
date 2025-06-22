using PersonalSite.Application.Features.Blogs.Blog.Dtos;

namespace PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;

public class BlogPostTagDtoValidator : AbstractValidator<BlogPostTagDto>
{
    public BlogPostTagDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .MaximumLength(50).WithMessage("Tag name must be 50 characters or fewer.");
    }
}
