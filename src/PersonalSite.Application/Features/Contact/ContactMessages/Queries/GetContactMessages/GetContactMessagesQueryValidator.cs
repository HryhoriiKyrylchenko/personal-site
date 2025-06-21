namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;

public class GetContactMessagesQueryValidator : AbstractValidator<GetContactMessagesQuery>
{
    public GetContactMessagesQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}