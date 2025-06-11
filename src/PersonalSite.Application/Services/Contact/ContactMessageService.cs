namespace PersonalSite.Application.Services.Contact;

public class ContactMessageService : 
    CrudServiceBase<ContactMessage, ContactMessageDto, ContactMessageAddRequest, ContactMessageUpdateRequest>, 
    IContactMessageService
{
    private IContactMessageRepository _contactMessageRepository;
    
    public ContactMessageService(
        IContactMessageRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CrudServiceBase<ContactMessage, ContactMessageDto, ContactMessageAddRequest, ContactMessageUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _contactMessageRepository = repository;
    }
    
    public override async Task<ContactMessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapContactMessageToDto(entity);
    }

    public override async Task<IReadOnlyList<ContactMessageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        return EntityToDtoMapper.MapContactMessagesToDtoList(entities);
    }

    public override async Task AddAsync(ContactMessageAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newMessage = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        
        await Repository.AddAsync(newMessage, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(ContactMessageUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingMessage = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if(existingMessage is null) throw new Exception("Message not found");

        existingMessage.Name = request.Name;
        existingMessage.Email = request.Email;
        existingMessage.Subject = request.Subject;
        existingMessage.Message = request.Message;
        
        await Repository.UpdateAsync(existingMessage, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if(entity is not null)
        {
            Repository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }   
    }

    public async Task<List<ContactMessageDto>> GetUnreadMessagesAsync(CancellationToken cancellationToken = default)
    {
        var unreadMessages = await _contactMessageRepository.GetUnreadMessagesAsync(cancellationToken);
        return EntityToDtoMapper.MapContactMessagesToDtoList(unreadMessages);
    }

    public async Task MarkMessageAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _contactMessageRepository.MarkAsReadAsync(id, cancellationToken);
    }

    public async Task MarkMessageAsUnreadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _contactMessageRepository.MarkAsUnreadAsync(id, cancellationToken);
    }
}