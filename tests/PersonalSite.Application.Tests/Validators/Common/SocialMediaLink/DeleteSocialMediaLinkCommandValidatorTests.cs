using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;

namespace PersonalSite.Application.Tests.Validators.Common.SocialMediaLink;

public class DeleteSocialMediaLinkCommandValidatorTests
{
    private readonly DeleteSocialMediaLinkCommandValidator _validator;

    public DeleteSocialMediaLinkCommandValidatorTests()
    {
        _validator = new DeleteSocialMediaLinkCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteSocialMediaLinkCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteSocialMediaLinkCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}