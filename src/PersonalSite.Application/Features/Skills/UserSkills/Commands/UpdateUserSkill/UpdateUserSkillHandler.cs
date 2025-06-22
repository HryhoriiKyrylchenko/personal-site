using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.UpdateUserSkill;

public class UpdateUserSkillHandler : IRequestHandler<UpdateUserSkillCommand, Result>
{
    private readonly IUserSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserSkillHandler> _logger;

    public UpdateUserSkillHandler(
        IUserSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserSkillHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateUserSkillCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            _logger.LogWarning($"User skill with id {request.Id} not found.");
            return Result.Failure("User skill not found.");
        }

        entity.Proficiency = request.Proficiency;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}