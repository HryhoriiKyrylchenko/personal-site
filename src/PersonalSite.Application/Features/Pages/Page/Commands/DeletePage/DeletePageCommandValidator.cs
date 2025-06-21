namespace PersonalSite.Application.Features.Pages.Page.Commands.DeletePage;

public class DeletePageCommandValidator : AbstractValidator<DeletePageCommand>
{
    public DeletePageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}