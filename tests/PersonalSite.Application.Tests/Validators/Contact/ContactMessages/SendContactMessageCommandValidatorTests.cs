using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;

namespace PersonalSite.Application.Tests.Validators.Contact.ContactMessages;

public class SendContactMessageCommandValidatorTests
{
    private readonly SendContactMessageCommandValidator _validator;

    public SendContactMessageCommandValidatorTests()
    {
        _validator = new SendContactMessageCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new SendContactMessageCommand("", "test@example.com", "Subject", "Message");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var longName = new string('a', 101);
        var command = new SendContactMessageCommand(longName, "test@example.com", "Subject", "Message");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must be 100 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var command = new SendContactMessageCommand("Name", "", "Subject", "Message");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Too_Long()
    {
        var longEmail = new string('a', 140) + "@example.com";
        var command = new SendContactMessageCommand("Name", longEmail, "Subject", "Message");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must be 150 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new SendContactMessageCommand("Name", "invalid-email", "Subject", "Message");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email must be a valid email address.");
    }

    [Fact]
    public void Should_Have_Error_When_Subject_Too_Long()
    {
        var longSubject = new string('a', 201);
        var command = new SendContactMessageCommand("Name", "test@example.com", longSubject, "Message");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Subject)
            .WithErrorMessage("Subject must be 200 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Message_Is_Empty()
    {
        var command = new SendContactMessageCommand("Name", "test@example.com", "Subject", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Message)
            .WithErrorMessage("Message is required.");
    }

    [Fact]
    public void Should_Have_Error_When_IpAddress_Too_Long()
    {
        var longIp = new string('1', 51);
        var command = new SendContactMessageCommand("Name", "test@example.com", "Subject", "Message")
        {
            IpAddress = longIp
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.IpAddress)
            .WithErrorMessage("IpAddress must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_UserAgent_Too_Long()
    {
        var longAgent = new string('a', 256);
        var command = new SendContactMessageCommand("Name", "test@example.com", "Subject", "Message")
        {
            UserAgent = longAgent
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.UserAgent)
            .WithErrorMessage("UserAgent must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var command = new SendContactMessageCommand("Name", "test@example.com", "Subject", "Message")
        {
            IpAddress = "127.0.0.1",
            UserAgent = "UnitTestAgent"
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}