using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinks;

public class GetSocialMediaLinksQueryHandler : IRequestHandler<GetSocialMediaLinksQuery, Result<List<SocialMediaLinkDto>>>
{
    private readonly ISocialMediaLinkRepository _repository;
    private readonly ILogger<GetSocialMediaLinksQueryHandler> _logger;
    private readonly IMapper<SocialMediaLink, SocialMediaLinkDto> _mapper;

    public GetSocialMediaLinksQueryHandler(
        ISocialMediaLinkRepository repository,
        ILogger<GetSocialMediaLinksQueryHandler> logger,
        IMapper<SocialMediaLink, SocialMediaLinkDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<List<SocialMediaLinkDto>>> Handle(GetSocialMediaLinksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var socialMediaLinks = await _repository.GetFilteredAsync(
                request.Platform, request.IsActive, cancellationToken);

            if (socialMediaLinks.IsFailure || socialMediaLinks.Value == null)
            {
                _logger.LogWarning("Social media links not found");
                return Result<List<SocialMediaLinkDto>>.Failure("Social media links not found");
            }
            
            var items = _mapper.MapToDtoList(socialMediaLinks.Value);
            
            return Result<List<SocialMediaLinkDto>>.Success(items);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting social media links");
            return Result<List<SocialMediaLinkDto>>.Failure("Failed to get social media links");      
        }
    }
}