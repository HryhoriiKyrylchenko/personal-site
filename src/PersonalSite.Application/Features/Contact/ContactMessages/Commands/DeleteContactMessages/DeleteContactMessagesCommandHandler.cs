using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.DeleteContactMessages;

public class DeleteContactMessagesCommandHandler : IRequestHandler<DeleteContactMessagesCommand, Result>
{
    private readonly IContactMessageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteContactMessagesCommandHandler> _logger;

    public DeleteContactMessagesCommandHandler(IContactMessageRepository repository, IUnitOfWork unitOfWork, ILogger<DeleteContactMessagesCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteContactMessagesCommand request, CancellationToken cancellationToken)
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
                _repository.Remove(msg);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contact messages.");
            return Result.Failure("Failed to delete messages.");
        }
    }
}