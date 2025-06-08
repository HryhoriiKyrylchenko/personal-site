namespace PersonalSite.Application.Services.Translations;

public class ProjectTranslationService : 
    CrudServiceBase<ProjectTranslation, ProjectTranslationDto, ProjectTranslationAddRequest, ProjectTranslationUpdateRequest>, 
    IProjectTranslationService
{
    IProjectTranslationRepository _projectTranslationRepository;
    
    public ProjectTranslationService(
        IProjectTranslationRepository repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
        _projectTranslationRepository = repository;   
    }

    public override async Task<ProjectTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var projectTranslation = await _projectTranslationRepository.GetWithLanguageByIdAsync(id, cancellationToken);
        return projectTranslation == null
            ? null
            : EntityToDtoMapper.MapProjectTranslationToDto(projectTranslation);
    }

    public override async Task<IReadOnlyList<ProjectTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projectTranslations = await _projectTranslationRepository.ListWithLanguageAsync(cancellationToken);
        
        return EntityToDtoMapper.MapProjectTranslationsToDtoList(projectTranslations);
    }

    public override async Task AddAsync(ProjectTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        var newProjectTranslation = new ProjectTranslation
        {
            Id = Guid.NewGuid(),
            LanguageId = request.LanguageId,
            ProjectId = request.ProjectId,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            DescriptionSections = request.DescriptionSections,
            MetaTitle = request.MetaTitle,
            MetaDescription = request.MetaDescription,
            OgImage = request.OgImage
        };
        
        await Repository.AddAsync(newProjectTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(ProjectTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingProjectTranslation = await Repository.GetByIdAsync(request.Id, cancellationToken);
        if (existingProjectTranslation is null) throw new Exception("Project translation not found");
        
        existingProjectTranslation.Title = request.Title;
        existingProjectTranslation.ShortDescription = request.ShortDescription;
        existingProjectTranslation.DescriptionSections = request.DescriptionSections;
        existingProjectTranslation.MetaTitle = request.MetaTitle;
        existingProjectTranslation.MetaDescription = request.MetaDescription;
        existingProjectTranslation.OgImage = request.OgImage;
        
        Repository.Update(existingProjectTranslation);
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
}