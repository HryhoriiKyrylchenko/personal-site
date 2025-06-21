namespace PersonalSite.Application.Features.Projects.Project.Commands.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProjectHandler> _logger;

    public DeleteProjectHandler(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateProjectHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (project == null)
            {
                _logger.LogWarning($"Project with ID {request.Id} not found.");
                return Result.Failure($"Project with ID {request.Id} not found.");
            }

            project.IsDeleted = true;
            project.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(project, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting project.");
            return Result.Failure("Error deleting project.");      
        }
    }
}
