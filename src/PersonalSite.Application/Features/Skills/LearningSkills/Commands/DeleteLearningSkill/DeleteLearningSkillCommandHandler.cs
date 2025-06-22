using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;

public class DeleteLearningSkillCommandHandler : IRequestHandler<DeleteLearningSkillCommand, Result>
{
    private readonly ILearningSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLearningSkillCommandHandler> _logger;

    public DeleteLearningSkillCommandHandler(
        ILearningSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteLearningSkillCommandHandler> logger)
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