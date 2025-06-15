namespace PersonalSite.Application.Services.Common.Validators;

public class SocialMediaLinkUpdateRequestValidator : AbstractValidator<SocialMediaLinkUpdateRequest>
{
    public SocialMediaLinkUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("SocialMediaLink ID is required.");

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