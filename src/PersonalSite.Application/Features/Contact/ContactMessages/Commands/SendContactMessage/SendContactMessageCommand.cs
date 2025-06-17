namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;

public class SendContactMessageCommand : IRequest<Result>
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}