namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;

public class GetPageByIdQueryValidator : AbstractValidator<GetPageByIdQuery>
{
    public GetPageByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");       
    }
}