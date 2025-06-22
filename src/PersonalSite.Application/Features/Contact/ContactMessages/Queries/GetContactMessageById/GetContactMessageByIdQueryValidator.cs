namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;

public class GetContactMessageByIdQueryValidator : AbstractValidator<GetContactMessageByIdQuery>
{
    public GetContactMessageByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");       
    }
}