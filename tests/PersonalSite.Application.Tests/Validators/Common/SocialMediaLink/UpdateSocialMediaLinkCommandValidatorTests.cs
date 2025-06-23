using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.UpdateSocialMediaLink;

namespace PersonalSite.Application.Tests.Validators.Common.SocialMediaLink;

public class UpdateSocialMediaLinkCommandValidatorTests
{
    private readonly UpdateSocialMediaLinkCommandValidator _validator;

    public UpdateSocialMediaLinkCommandValidatorTests()
    {
        _validator = new UpdateSocialMediaLinkCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateSocialMediaLinkCommand(
            Guid.Empty,
            "Facebook",
            "https://valid.url",
            1,
            true);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Have_Error_When_Url_Is_Null_Or_Empty(string url)
    {
        var command = new UpdateSocialMediaLinkCommand(
            Guid.NewGuid(),
            "Facebook",
            url,
            1,
            true);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Url);
    }

    [Fact]
    public void Should_Have_Error_When_Url_Is_Invalid()
    {
        var command = new UpdateSocialMediaLinkCommand(
            Guid.NewGuid(),
            "Facebook",
            "invalid_url",
            1,
            true);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Url);
    }

    [Fact]
    public void Should_Have_Error_When_Url_Too_Long()
    {
        var longUrl = new string('a', 256);
        var command = new UpdateSocialMediaLinkCommand(
            Guid.NewGuid(),
            "Facebook",
            longUrl,
            1,
            true);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Url);
    }

    [Fact]
    public void Should_Have_Error_When_DisplayOrder_Is_Negative()
    {
        var command = new UpdateSocialMediaLinkCommand(
            Guid.NewGuid(),
            "Facebook",
            "https://valid.url",
            -1,
            true);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.DisplayOrder);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Properties_Are_Valid()
    {
        var command = new UpdateSocialMediaLinkCommand(
            Guid.NewGuid(),
            "Facebook",
            "https://valid.url",
            0,
            true);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.Url);
        result.ShouldNotHaveValidationErrorFor(c => c.DisplayOrder);
    }
}