using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Resume.Commands.UpdateResume;

namespace PersonalSite.Application.Tests.Validators.Common.Resume;

public class UpdateResumeCommandValidatorTests
{
    private readonly UpdateResumeCommandValidator _validator;

    public UpdateResumeCommandValidatorTests()
    {
        _validator = new UpdateResumeCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateResumeCommand(Guid.Empty, "http://file.url", "filename.txt", true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Resume ID is required.");
    }

    [Fact]
    public void Should_Have_Error_When_FileUrl_Is_Empty()
    {
        var command = new UpdateResumeCommand(Guid.NewGuid(), string.Empty, "filename.txt", true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FileUrl)
            .WithErrorMessage("FileUrl is required.");
    }

    [Fact]
    public void Should_Have_Error_When_FileUrl_Too_Long()
    {
        var longUrl = new string('a', 256);
        var command = new UpdateResumeCommand(Guid.NewGuid(), longUrl, "filename.txt", true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FileUrl)
            .WithErrorMessage("FileUrl must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_FileName_Too_Long()
    {
        var longName = new string('a', 256);
        var command = new UpdateResumeCommand(Guid.NewGuid(), "http://file.url", longName, true);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FileName)
            .WithErrorMessage("FileName must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid_Command()
    {
        var command = new UpdateResumeCommand(Guid.NewGuid(), "http://file.url", "filename.txt", true);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}