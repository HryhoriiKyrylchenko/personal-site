using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinkById;

namespace PersonalSite.Application.Tests.Validators.Common.SocialMediaLink;

public class GetSocialMediaLinkByIdQueryValidatorTests
{
    private readonly GetSocialMediaLinkByIdQueryValidator _validator;

    public GetSocialMediaLinkByIdQueryValidatorTests()
    {
        _validator = new GetSocialMediaLinkByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetSocialMediaLinkByIdQuery(Guid.Empty);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("SocialMediaLink ID is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var query = new GetSocialMediaLinkByIdQuery(Guid.NewGuid());

        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}