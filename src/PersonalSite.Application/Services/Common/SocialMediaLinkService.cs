namespace PersonalSite.Application.Services.Common;

public class SocialMediaLinkService : 
    CrudServiceBase<SocialMediaLink, SocialMediaLinkDto, SocialMediaLinkAddRequest, SocialMediaLinkUpdateRequest>, 
    ISocialMediaLinkService
{
    ISocialMediaLinkRepository _socialMediaLinkRepository;
    
    public SocialMediaLinkService(
        ISocialMediaLinkRepository repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
        _socialMediaLinkRepository = repository;
    }

    public override async Task<SocialMediaLinkDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapSocialMediaLinkToDto(entity);
    }

    public override async Task<IReadOnlyList<SocialMediaLinkDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        
        return EntityToDtoMapper.MapSocialMediaLinksToDtoList(entities);   
    }

    public override async Task AddAsync(SocialMediaLinkAddRequest request, CancellationToken cancellationToken = default)
    {
        var newSocialMediaLink = new SocialMediaLink
        {
            Id = Guid.NewGuid(),
            Platform = request.Platform,
            Url = request.Url,
            DisplayOrder = request.DisplayOrder,
            IsActive = true
        };
        
        await Repository.AddAsync(newSocialMediaLink, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken); 
    }

    public override async Task UpdateAsync(SocialMediaLinkUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingSocialMediaLink = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingSocialMediaLink is null) throw new Exception("Social media link is not found");
        
        existingSocialMediaLink.Url = request.Url;
        existingSocialMediaLink.DisplayOrder = request.DisplayOrder;
        existingSocialMediaLink.IsActive = request.IsActive;
        
        await Repository.UpdateAsync(existingSocialMediaLink, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            Repository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<SocialMediaLinkDto>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _socialMediaLinkRepository.GetAllActiveAsync(cancellationToken);
        
        return EntityToDtoMapper.MapSocialMediaLinksToDtoList(entities);
    }

    public async Task<SocialMediaLinkDto?> GetByPlatformAsync(string platform, CancellationToken cancellationToken = default)
    {
        var entity = await _socialMediaLinkRepository.GetActiveByPlatformAsync(platform, cancellationToken);

        return entity == null ? null : EntityToDtoMapper.MapSocialMediaLinkToDto(entity);
    }
}