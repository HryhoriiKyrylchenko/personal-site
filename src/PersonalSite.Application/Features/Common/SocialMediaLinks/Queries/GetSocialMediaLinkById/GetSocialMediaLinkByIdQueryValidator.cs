namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinkById;

public class GetSocialMediaLinkByIdQueryValidator : AbstractValidator<GetSocialMediaLinkByIdQuery>
{
    public GetSocialMediaLinkByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("SocialMediaLink ID is required.");       
    }
}