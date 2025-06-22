using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinks;

public class GetSocialMediaLinksHandler : IRequestHandler<GetSocialMediaLinksQuery, Result<List<SocialMediaLinkDto>>>
{
    private readonly ISocialMediaLinkRepository _repository;
    private readonly ILogger<GetSocialMediaLinksHandler> _logger;
    private readonly IMapper<SocialMediaLink, SocialMediaLinkDto> _mapper;

    public GetSocialMediaLinksHandler(
        ISocialMediaLinkRepository repository,
        ILogger<GetSocialMediaLinksHandler> logger,
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
            var query = _repository.GetQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Platform))
                query = query.Where(x => x.Platform.Contains(request.Platform));

            if (request.IsActive.HasValue)
                query = query.Where(x => x.IsActive == request.IsActive.Value);

            var entities = await query
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken);

            var items = _mapper.MapToDtoList(entities);
            
            return Result<List<SocialMediaLinkDto>>.Success(items);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting social media links");
            return Result<List<SocialMediaLinkDto>>.Failure("Failed to get social media links");      
        }
    }
}