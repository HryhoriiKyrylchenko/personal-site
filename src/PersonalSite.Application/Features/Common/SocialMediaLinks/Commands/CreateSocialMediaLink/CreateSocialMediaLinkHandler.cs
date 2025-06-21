namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;

public class CreateSocialMediaLinkHandler : IRequestHandler<CreateSocialMediaLinkCommand, Result<Guid>>
{
    private readonly ISocialMediaLinkRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSocialMediaLinkHandler> _logger;

    public CreateSocialMediaLinkHandler(
        ISocialMediaLinkRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateSocialMediaLinkHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;   
    }

    public async Task<Result<Guid>> Handle(CreateSocialMediaLinkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new SocialMediaLink
            {
                Id = Guid.NewGuid(),
                Platform = request.Platform,
                Url = request.Url,
                DisplayOrder = request.DisplayOrder,
                IsActive = request.IsActive
            };

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating social media link.");
            return Result<Guid>.Failure("Failed to create social media link.");      
        }
    }
}
