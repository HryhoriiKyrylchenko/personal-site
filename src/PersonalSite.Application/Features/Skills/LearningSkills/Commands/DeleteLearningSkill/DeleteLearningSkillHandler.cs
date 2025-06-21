namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;

public class DeleteLearningSkillHandler : IRequestHandler<DeleteLearningSkillCommand, Result>
{
    private readonly ILearningSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLearningSkillHandler> _logger;

    public DeleteLearningSkillHandler(
        ILearningSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteLearningSkillHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteLearningSkillCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            _logger.LogWarning("Learning skill not found.");
            return Result.Failure("Learning skill not found.");
        }

        entity.IsDeleted = true;
        
        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}