namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;

public class CreateSocialMediaLinkCommandValidator : AbstractValidator<CreateSocialMediaLinkCommand>
{
    public CreateSocialMediaLinkCommandValidator()
    {
        RuleFor(x => x.Platform)
            .NotEmpty().WithMessage("Platform is required.")
            .MaximumLength(50).WithMessage("Platform must be 50 characters or fewer.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required.")
            .MaximumLength(255).WithMessage("Url must be 255 characters or fewer.")
            .Must(BeAValidUrl).WithMessage("Url must be a valid URL.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder cannot be negative.");
    }
    
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var _);
    }
}