using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Resume.Commands.DeleteResume;

public class DeleteResumeCommandHandler : IRequestHandler<DeleteResumeCommand, Result>
{
    private readonly IResumeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    ILogger<DeleteResumeCommandHandler> _logger;

    public DeleteResumeCommandHandler(
        IResumeRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteResumeCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger; 
    }

    public async Task<Result> Handle(DeleteResumeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resume = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (resume == null)
                return Result.Failure("Resume not found.");

            _repository.Remove(resume);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting resume.");
            return Result.Failure("Failed to delete resume.");       
        }
    }
}