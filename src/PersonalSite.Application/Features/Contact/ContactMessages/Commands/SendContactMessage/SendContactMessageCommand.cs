namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;

public record SendContactMessageCommand(
    string Name,
    string Email,
    string Subject,
    string Message
) : IRequest<Result>
{
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}