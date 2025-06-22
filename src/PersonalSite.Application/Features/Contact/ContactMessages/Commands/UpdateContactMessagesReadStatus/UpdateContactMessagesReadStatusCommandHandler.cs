using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;

public class UpdateContactMessagesReadStatusCommandHandler : IRequestHandler<UpdateContactMessagesReadStatusCommand, Result>
{
    private readonly IContactMessageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateContactMessagesReadStatusCommandHandler> _logger;

    public UpdateContactMessagesReadStatusCommandHandler(IContactMessageRepository repository, IUnitOfWork unitOfWork, ILogger<UpdateContactMessagesReadStatusCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateContactMessagesReadStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var messages = await _repository.GetByIdsAsync(request.Ids, cancellationToken);

            if (messages.Count != request.Ids.Count)
            {
                return Result.Failure("Some messages were not found.");
            }

            foreach (var msg in messages)
            {
                msg.IsRead = request.IsRead;
                await _repository.UpdateAsync(msg, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating read status of contact messages.");
            return Result.Failure("Failed to update read status.");
        }
    }
}