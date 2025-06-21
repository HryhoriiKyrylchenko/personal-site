namespace PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;

public class DeleteLanguageHandler : IRequestHandler<DeleteLanguageCommand, Result>
{
    private readonly ILanguageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLanguageHandler> _logger;   

    public DeleteLanguageHandler(
        ILanguageRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteLanguageHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;  
    }

    public async Task<Result> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var language = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (language == null)
            {
                _logger.LogWarning("Language not found.");  
                return Result.Failure("Language not found.");
            }

            language.IsDeleted = true;
            
            await _repository.UpdateAsync(language, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting language");
            return Result.Failure("Error occurred while deleting language");      
        }
    }
}