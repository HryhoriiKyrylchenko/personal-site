using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;

namespace PersonalSite.Application.Tests.Validators.Common.Resume;

public class CreateResumeCommandValidatorTests
{
    private readonly CreateResumeCommandValidator _validator;

    public CreateResumeCommandValidatorTests()
    {
        _validator = new CreateResumeCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_FileUrl_Is_Null_Or_Empty()
    {
        var command = new CreateResumeCommand("", "filename.pdf", true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FileUrl);
    }

    [Fact]
    public void Should_Have_Error_When_FileUrl_Exceeds_MaxLength()
    {
        var longUrl = new string('a', 256);
        var command = new CreateResumeCommand(longUrl, "filename.pdf", true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FileUrl);
    }

    [Fact]
    public void Should_Have_Error_When_FileName_Is_Null_Or_Empty()
    {
        var command = new CreateResumeCommand("fileurl", "", true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FileName);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid_Command()
    {
        var command = new CreateResumeCommand("fileurl", "filename.pdf", true);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.FileUrl);
        result.ShouldNotHaveValidationErrorFor(x => x.FileName);
    }
}