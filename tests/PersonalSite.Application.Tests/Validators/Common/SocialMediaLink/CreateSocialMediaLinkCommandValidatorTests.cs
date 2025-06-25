using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;

namespace PersonalSite.Application.Tests.Validators.Common.SocialMediaLink;

public class CreateSocialMediaLinkCommandValidatorTests
{
    private readonly CreateSocialMediaLinkCommandValidator _validator;

    public CreateSocialMediaLinkCommandValidatorTests()
    {
        _validator = new CreateSocialMediaLinkCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Platform_Is_Empty()
    {
        var command = new CreateSocialMediaLinkCommand("", "https://valid.url", 1, true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Platform)
            .WithErrorMessage("Platform is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Platform_Too_Long()
    {
        var platform = new string('a', 51);
        var command = new CreateSocialMediaLinkCommand(platform, "https://valid.url", 1, true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Platform)
            .WithErrorMessage("Platform must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Url_Is_Empty()
    {
        var command = new CreateSocialMediaLinkCommand("Twitter", "", 1, true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Url)
            .WithErrorMessage("Url is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Url_Too_Long()
    {
        var url = "http://" + new string('a', 250) + ".com";
        var command = new CreateSocialMediaLinkCommand("Twitter", url, 1, true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Url)
            .WithErrorMessage("Url must be 255 characters or fewer.");
    }

    [Theory]
    [InlineData("not-a-url", false)]
    [InlineData("htp:/wrong.com", false)]
    [InlineData("http://valid.com", true)]
    [InlineData("https://valid.com", true)]
    public void BeAValidUrl_Works_Correctly(string url, bool expected)
    {
        var validator = new CreateSocialMediaLinkCommandValidator();

        var method = typeof(CreateSocialMediaLinkCommandValidator)
            .GetMethod("BeAValidUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(method); // This will fail the test early if the method is missing

        var actual = (bool)method!.Invoke(validator, new object[] { url })!;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_Have_Error_When_DisplayOrder_Is_Negative()
    {
        var command = new CreateSocialMediaLinkCommand("Twitter", "https://valid.url", -1, true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.DisplayOrder)
            .WithErrorMessage("DisplayOrder cannot be negative.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var command = new CreateSocialMediaLinkCommand("Twitter", "https://valid.url", 0, true);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}