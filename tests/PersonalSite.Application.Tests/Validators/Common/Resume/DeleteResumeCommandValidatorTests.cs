using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Resume.Commands.DeleteResume;

namespace PersonalSite.Application.Tests.Validators.Common.Resume;

public class DeleteResumeCommandValidatorTests
{
    private readonly DeleteResumeCommandValidator _validator;

    public DeleteResumeCommandValidatorTests()
    {
        _validator = new DeleteResumeCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteResumeCommand(Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteResumeCommand(Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}