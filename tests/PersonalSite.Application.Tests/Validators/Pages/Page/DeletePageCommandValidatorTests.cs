using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Pages.Page.Commands.DeletePage;

namespace PersonalSite.Application.Tests.Validators.Pages.Page;

public class DeletePageCommandValidatorTests
{
    private readonly DeletePageCommandValidator _validator;

    public DeletePageCommandValidatorTests()
    {
        _validator = new DeletePageCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeletePageCommand(Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Not_Empty()
    {
        var command = new DeletePageCommand(Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}