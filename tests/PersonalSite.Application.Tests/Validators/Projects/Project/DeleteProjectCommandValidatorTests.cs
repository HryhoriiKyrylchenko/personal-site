using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Projects.Project.Commands.DeleteProject;

namespace PersonalSite.Application.Tests.Validators.Projects.Project;

public class DeleteProjectCommandValidatorTests
{
    private readonly DeleteProjectCommandValidator _validator;

    public DeleteProjectCommandValidatorTests()
    {
        _validator = new DeleteProjectCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new DeleteProjectCommand(Guid.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var model = new DeleteProjectCommand(Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}