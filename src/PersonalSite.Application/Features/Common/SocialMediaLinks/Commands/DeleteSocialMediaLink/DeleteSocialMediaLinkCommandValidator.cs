namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;

public class DeleteSocialMediaLinkCommandValidator : AbstractValidator<DeleteSocialMediaLinkCommand>
{
    public DeleteSocialMediaLinkCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}