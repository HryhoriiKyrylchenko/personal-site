namespace PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;

public class UpdateLanguageHandler : IRequestHandler<UpdateLanguageCommand, Result>
{
    private readonly ILanguageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLanguageHandler> _logger;   

    public UpdateLanguageHandler(
        ILanguageRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<UpdateLanguageHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;       
    }

    public async Task<Result> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var language = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (language == null)
            {
                _logger.LogWarning("Language not found.");   
                return Result.Failure("Language not found.");
            }

            language.Code = request.Code;
            language.Name = request.Name;

            await _repository.UpdateAsync(language, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating language");
            return Result.Failure("Error occurred while updating language");      
        }
    }
}
