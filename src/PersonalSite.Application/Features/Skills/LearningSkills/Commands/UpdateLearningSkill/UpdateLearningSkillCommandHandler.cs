using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.UpdateLearningSkill;

public class UpdateLearningSkillCommandHandler : IRequestHandler<UpdateLearningSkillCommand, Result>
{
    private readonly ILearningSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLearningSkillCommandHandler> _logger;

    public UpdateLearningSkillCommandHandler(
        ILearningSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateLearningSkillCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateLearningSkillCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            _logger.LogWarning("Learning skill not found.");
            return Result.Failure("Learning skill not found.");
        }

        entity.LearningStatus = request.LearningStatus;
        entity.DisplayOrder = request.DisplayOrder;

        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}