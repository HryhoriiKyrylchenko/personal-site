using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Language.Commands.CreateLanguage;

public class CreateLanguageHandler : IRequestHandler<CreateLanguageCommand, Result<Guid>>
{
    private readonly ILanguageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLanguageHandler> _logger;

    public CreateLanguageHandler(
        ILanguageRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<CreateLanguageHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;   
    }

    public async Task<Result<Guid>> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _repository.ExistsByCodeAsync(request.Code, cancellationToken);
            if (exists)
            {
                _logger.LogWarning($"Language code {request.Code} already exists.");
                return Result<Guid>.Failure($"Language code {request.Code} already exists.");
            }

            var language = new Domain.Entities.Common.Language
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                Name = request.Name
            };

            await _repository.AddAsync(language, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(language.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while creating language");
            return Result<Guid>.Failure("Error occurred while creating language");      
        }
    }
}
