namespace PersonalSite.Application.Features.Pages.Page.Commands.DeletePage;

public record DeletePageCommand(Guid Id) : IRequest<Result>;