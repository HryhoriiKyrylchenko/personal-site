using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.DeleteContactMessages;

namespace PersonalSite.Application.Tests.Validators.Contact.ContactMessages;

public class DeleteContactMessagesCommandValidatorTests
{
    private readonly DeleteContactMessagesCommandValidator _validator;

    public DeleteContactMessagesCommandValidatorTests()
    {
        _validator = new DeleteContactMessagesCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Ids_Is_Empty()
    {
        var command = new DeleteContactMessagesCommand(new List<Guid>());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Ids)
            .WithErrorMessage("At least one message Id must be provided.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Ids_Has_Values()
    {
        var command = new DeleteContactMessagesCommand(new List<Guid> { Guid.NewGuid() });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Ids);
    }
}