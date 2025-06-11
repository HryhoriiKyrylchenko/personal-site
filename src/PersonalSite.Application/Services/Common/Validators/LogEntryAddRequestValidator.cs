namespace PersonalSite.Application.Services.Common.Validators;

public class LogEntryAddRequestValidator : AbstractValidator<LogEntryAddRequest>
{
    public LogEntryAddRequestValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .MaximumLength(128).WithMessage("Level must be 128 characters or fewer.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.");

        RuleFor(x => x.MessageTemplate)
            .NotEmpty().WithMessage("MessageTemplate is required.");

        RuleFor(x => x.Properties)
            .Must(BeValidJson).When(x => !string.IsNullOrWhiteSpace(x.Properties))
            .WithMessage("Properties must be valid JSON.");

        RuleFor(x => x.SourceContext)
            .MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.SourceContext));
    }

    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            System.Text.Json.JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}