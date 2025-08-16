using PersonalSite.Application.Features.Contact.ContactMessages.Events.ContactMessageCreated;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;

public class SendContactMessageCommandHandler : IRequestHandler<SendContactMessageCommand, Result>
{
    private readonly IContactMessageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<SendContactMessageCommandHandler> _logger;
    
    public SendContactMessageCommandHandler(
        IContactMessageRepository repository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<SendContactMessageCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<Result> Handle(SendContactMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var contactMessage = new ContactMessage
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Subject = request.Subject,
                Message = request.Message,
                UserAgent = request.UserAgent,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            
            await _repository.AddAsync(contactMessage, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Contact message created successfully.");
            
            await _mediator.Publish(new ContactMessageCreatedEvent(contactMessage), cancellationToken);
            
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating contact message.");
            return Result.Failure("Error creating contact message.");       
        }
    }
}