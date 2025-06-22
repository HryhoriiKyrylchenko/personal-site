using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Commands.DeletePage;

public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, Result>
{
    private readonly IPageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePageCommandHandler> _logger;

    public DeletePageCommandHandler(
        IPageRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeletePageCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;       
    }

    public async Task<Result> Handle(DeletePageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var page = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (page is null)
            {
                _logger.LogWarning($"Page {request.Id} not found.");
                return Result.Failure("Page not found.");
            }

            page.IsDeleted = true;
            
            await _repository.UpdateAsync(page, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting page.");
            return Result.Failure("Error deleting page.");       
        }
    }
}
