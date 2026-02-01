using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;

public class CreateResumeCommandHandler : IRequestHandler<CreateResumeCommand, Result<Guid>>
{
    private readonly IResumeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateResumeCommandHandler> _logger;
    private readonly IS3UrlBuilder _urlBuilder;

    public CreateResumeCommandHandler(
        IResumeRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateResumeCommandHandler> logger,
        IS3UrlBuilder urlBuilder)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;   
        _urlBuilder = urlBuilder;
    }

    public async Task<Result<Guid>> Handle(CreateResumeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resume = new Domain.Entities.Common.Resume
            {
                Id = Guid.NewGuid(),
                FileUrl = string.IsNullOrWhiteSpace(request.FileUrl)
                    ? string.Empty
                    : _urlBuilder.ExtractKey(request.FileUrl),
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
