using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Features.Projects.Project.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProjectCommandHandler> _logger;

    public DeleteProjectCommandHandler(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateProjectCommandHandler> logger)
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
