using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;

public class DeleteSocialMediaLinkCommandHandler : IRequestHandler<DeleteSocialMediaLinkCommand, Result>
{
    private readonly ISocialMediaLinkRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    ILogger<DeleteSocialMediaLinkCommandHandler> _logger;

    public DeleteSocialMediaLinkCommandHandler(
        ISocialMediaLinkRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteSocialMediaLinkCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteSocialMediaLinkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Result.Failure("Social media link not found.");

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting social media link.");
            return Result.Failure("An unexpected error occurred.");      
        }
    }
}
