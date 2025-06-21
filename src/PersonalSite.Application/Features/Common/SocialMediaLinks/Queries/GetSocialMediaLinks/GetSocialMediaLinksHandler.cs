namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinks;

public class GetSocialMediaLinksHandler : IRequestHandler<GetSocialMediaLinksQuery, List<SocialMediaLinkDto>>
{
    private readonly ISocialMediaLinkRepository _repository;

    public GetSocialMediaLinksHandler(ISocialMediaLinkRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SocialMediaLinkDto>> Handle(GetSocialMediaLinksQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Platform))
            query = query.Where(x => x.Platform.Contains(request.Platform));

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        var entities = await query
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);

        return SocialMediaLinkMapper.MapToDtoList(entities);
    }
}