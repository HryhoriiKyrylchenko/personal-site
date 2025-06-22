namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;

public class GetContactMessagesHandler : IRequestHandler<GetContactMessagesQuery, PaginatedResult<ContactMessageDto>>
{
    private readonly IContactMessageRepository _repository;
    private readonly ILogger<GetContactMessagesHandler> _logger;
    private readonly IMapper<ContactMessage, ContactMessageDto> _mapper;

    public GetContactMessagesHandler(
        IContactMessageRepository repository,
        ILogger<GetContactMessagesHandler> logger,
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
            var query = _repository.GetQueryable();

            if (request.IsReadFilter.HasValue)
                query = query.Where(x => x.IsRead == request.IsReadFilter.Value);

            var total = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        
            var items = _mapper.MapToDtoList(entities);

            return PaginatedResult<ContactMessageDto>.Success(items, total, request.Page, request.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting contact messages.");
            return PaginatedResult<ContactMessageDto>.Failure("Error getting contact messages.");       
        }
    }
}