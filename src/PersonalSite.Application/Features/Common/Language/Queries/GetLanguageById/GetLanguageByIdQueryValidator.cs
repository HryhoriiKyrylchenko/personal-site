namespace PersonalSite.Application.Features.Common.Language.Queries.GetLanguageById;

public class GetLanguageByIdQueryValidator : AbstractValidator<GetLanguageByIdQuery>
{
    public GetLanguageByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Language ID is required.");
    }
}