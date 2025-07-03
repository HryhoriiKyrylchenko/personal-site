using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;

public class CreateResumeCommandHandler : IRequestHandler<CreateResumeCommand, Result<Guid>>
{
    private readonly IResumeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateResumeCommandHandler> _logger;

    public CreateResumeCommandHandler(
        IResumeRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateResumeCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;   
    }

    public async Task<Result<Guid>> Handle(CreateResumeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resume = new Domain.Entities.Common.Resume
            {
                Id = Guid.NewGuid(),
                FileUrl = request.FileUrl,
                FileName = request.FileName,
                UploadedAt = DateTime.UtcNow,
                IsActive = request.IsActive
            };

            await _repository.AddAsync(resume, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(resume.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating resume.");
            return Result<Guid>.Failure("Failed to create resume.");       
        }
    }
}
