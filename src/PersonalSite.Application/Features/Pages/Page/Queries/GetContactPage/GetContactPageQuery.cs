using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetContactPage;

public record GetContactPageQuery : IRequest<Result<ContactPageDto>>;