using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;

public class GetContactMessageByIdQueryHandler : IRequestHandler<GetContactMessageByIdQuery, Result<ContactMessageDto>>
{
    private readonly IContactMessageRepository _repository;
    private readonly ILogger<GetContactMessageByIdQueryHandler> _logger;
    private readonly IMapper<ContactMessage, ContactMessageDto> _mapper;
    
    public GetContactMessageByIdQueryHandler(
        IContactMessageRepository repository,
        ILogger<GetContactMessageByIdQueryHandler> logger,
        IMapper<ContactMessage, ContactMessageDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<ContactMessageDto>> Handle(GetContactMessageByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Contact message not found.");   
                return Result<ContactMessageDto>.Failure("Contact message not found.");
            }
            var dto = _mapper.MapToDto(entity);
            return Result<ContactMessageDto>.Success(dto);       
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting contact message by id.");
            return Result<ContactMessageDto>.Failure("Error getting contact message by id.");       
        }
    }
}