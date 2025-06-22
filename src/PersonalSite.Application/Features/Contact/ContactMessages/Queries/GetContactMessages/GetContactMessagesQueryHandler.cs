using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;

public class GetContactMessagesQueryHandler : IRequestHandler<GetContactMessagesQuery, PaginatedResult<ContactMessageDto>>
{
    private readonly IContactMessageRepository _repository;
    private readonly ILogger<GetContactMessagesQueryHandler> _logger;
    private readonly IMapper<ContactMessage, ContactMessageDto> _mapper;

    public GetContactMessagesQueryHandler(
        IContactMessageRepository repository,
        ILogger<GetContactMessagesQueryHandler> logger,
        IMapper<ContactMessage, ContactMessageDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;       
    }

    public async Task<PaginatedResult<ContactMessageDto>> Handle(GetContactMessagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var messages = await _repository.GetFilteredAsync(
                request.Page,
                request.PageSize,
                request.IsReadFilter,
                cancellationToken);

            if (messages.IsFailure || messages.Value == null)
            {
                _logger.LogWarning("Contact messages not found.");
                return PaginatedResult<ContactMessageDto>.Failure("Contact messages not found.");
            }
        
            var items = _mapper.MapToDtoList(messages.Value);;

            return PaginatedResult<ContactMessageDto>.Success(items, messages.PageNumber, messages.PageSize, messages.TotalCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting contact messages.");
            return PaginatedResult<ContactMessageDto>.Failure("Error getting contact messages.");       
        }
    }
}