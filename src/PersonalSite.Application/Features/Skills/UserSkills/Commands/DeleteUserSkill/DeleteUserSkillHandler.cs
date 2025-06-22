using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.DeleteUserSkill;

public class DeleteUserSkillHandler : IRequestHandler<DeleteUserSkillCommand, Result>
{
    private readonly IUserSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteUserSkillHandler> _logger;

    public DeleteUserSkillHandler(
        IUserSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteUserSkillHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteUserSkillCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            _logger.LogWarning("User skill not found.");           
            return Result.Failure("User skill not found.");
        }

        entity.IsDeleted = true;
        
        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}