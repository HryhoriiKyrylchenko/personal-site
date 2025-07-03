using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;

namespace PersonalSite.Application.Tests.Validators.Contact.ContactMessages;

public class UpdateContactMessagesReadStatusCommandValidatorTests
{
    private readonly UpdateContactMessagesReadStatusCommandValidator _validator;

    public UpdateContactMessagesReadStatusCommandValidatorTests()
    {
        _validator = new UpdateContactMessagesReadStatusCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Ids_Is_Empty()
    {
        var command = new UpdateContactMessagesReadStatusCommand(new List<Guid>(), true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Ids)
            .WithErrorMessage("At least one message Id must be provided.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Ids_Is_Not_Empty()
    {
        var command = new UpdateContactMessagesReadStatusCommand([Guid.NewGuid()], true);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Ids);
    }
}