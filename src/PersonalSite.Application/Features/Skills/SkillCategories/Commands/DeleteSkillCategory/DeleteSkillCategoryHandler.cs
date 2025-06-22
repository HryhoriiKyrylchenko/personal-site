using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;

public class DeleteSkillCategoryHandler : IRequestHandler<DeleteSkillCategoryCommand, Result>
{
    private readonly ISkillCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSkillCategoryHandler> _logger;

    public DeleteSkillCategoryHandler(
        ISkillCategoryRepository repository, 
        IUnitOfWork unitOfWork, 
        ILogger<DeleteSkillCategoryHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteSkillCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var skillCategory = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (skillCategory == null)
                return Result.Failure("Skill category not found.");

            _repository.Remove(skillCategory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting skill category.");
            return Result.Failure("Error occurred while deleting skill category.");
        }
    }
}