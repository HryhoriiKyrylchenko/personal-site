namespace PersonalSite.Application.Services.Translations;

public class LanguageService : 
    CrudServiceBase<Language, LanguageDto, LanguageAddRequest, LanguageUpdateRequest>, 
    ILanguageService
{
    ILanguageRepository _languageRepository;
    
    public LanguageService(
        ILanguageRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CrudServiceBase<Language, LanguageDto, LanguageAddRequest, LanguageUpdateRequest>> logger,
        IServiceProvider serviceProvider) 
        : base(repository, unitOfWork, logger, serviceProvider)
    {
        _languageRepository = repository; 
    }

    public override async Task<LanguageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var language = await Repository.GetByIdAsync(id, cancellationToken);
        return language == null
            ? null
            : EntityToDtoMapper.MapLanguageToDto(language);
    }

    public override async Task<IReadOnlyList<LanguageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var languages = await Repository.ListAsync(cancellationToken);
        return EntityToDtoMapper.MapLanguagesToDtoList(languages);
    }

    public override async Task AddAsync(LanguageAddRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateAddRequestAsync(request, cancellationToken);
        
        var newLanguage = new Language
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name
        };
        
        await Repository.AddAsync(newLanguage, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(LanguageUpdateRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateUpdateRequestAsync(request, cancellationToken);
        
        var existingLanguage = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingLanguage is null) throw new Exception("Language not found");
        
        existingLanguage.Code = request.Code;
        existingLanguage.Name = request.Name;
        
        await Repository.UpdateAsync(existingLanguage, cancellationToken);
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

    public async Task<bool> IsSupportedAsync(string code, CancellationToken cancellationToken = default)
    {
        var language = await _languageRepository.GetByCodeAsync(code, cancellationToken);
        return language is not null;
    }
}