namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.UpdateSocialMediaLink;

public class UpdateSocialMediaLinkHandler : IRequestHandler<UpdateSocialMediaLinkCommand, Result>
{
    private readonly ISocialMediaLinkRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    ILogger<UpdateSocialMediaLinkHandler> _logger;

    public UpdateSocialMediaLinkHandler(
        ISocialMediaLinkRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<UpdateSocialMediaLinkHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;       
    }

    public async Task<Result> Handle(UpdateSocialMediaLinkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                return Result.Failure("Social media link not found.");

            entity.Platform = request.Platform;
            entity.Url = request.Url;
            entity.DisplayOrder = request.DisplayOrder;
            entity.IsActive = request.IsActive;

            await _repository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating social media link.");
            return Result.Failure("An unexpected error occurred.");      
        }
    }
}