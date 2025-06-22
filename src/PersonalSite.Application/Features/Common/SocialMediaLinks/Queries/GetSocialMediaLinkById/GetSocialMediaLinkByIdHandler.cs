using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinkById;

public class GetSocialMediaLinkByIdHandler : IRequestHandler<GetSocialMediaLinkByIdQuery, Result<SocialMediaLinkDto>>
{
    private readonly ISocialMediaLinkRepository _repository;
    private readonly ILogger<GetSocialMediaLinkByIdHandler> _logger;   
    private readonly IMapper<SocialMediaLink, SocialMediaLinkDto> _mapper;

    public GetSocialMediaLinkByIdHandler(
        ISocialMediaLinkRepository repository,
        ILogger<GetSocialMediaLinkByIdHandler> logger,
        IMapper<SocialMediaLink, SocialMediaLinkDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<SocialMediaLinkDto>> Handle(GetSocialMediaLinkByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Social media link with ID {Id} not found.", request.Id);
                return Result<SocialMediaLinkDto>.Failure("Social media link not found.");
            }
            var result = _mapper.MapToDto(entity);
            return Result<SocialMediaLinkDto>.Success(result);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while getting social media link by id.");
            return Result<SocialMediaLinkDto>.Failure(e.Message);       
        }
    }
}