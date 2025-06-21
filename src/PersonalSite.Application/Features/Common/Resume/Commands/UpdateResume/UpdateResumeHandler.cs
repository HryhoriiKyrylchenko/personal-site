namespace PersonalSite.Application.Features.Common.Resume.Commands.UpdateResume;

public class UpdateResumeHandler : IRequestHandler<UpdateResumeCommand, Result>
{
    private readonly IResumeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateResumeHandler> _logger;

    public UpdateResumeHandler(
        IResumeRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<UpdateResumeHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;  
    }

    public async Task<Result> Handle(UpdateResumeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resume = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (resume == null)
                return Result.Failure("Resume not found.");

            resume.FileUrl = request.FileUrl;
            resume.FileName = request.FileName;
            resume.IsActive = request.IsActive;

            await _repository.UpdateAsync(resume, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating resume.");
            return Result.Failure("Failed to update resume.");      
        }
    }
}
