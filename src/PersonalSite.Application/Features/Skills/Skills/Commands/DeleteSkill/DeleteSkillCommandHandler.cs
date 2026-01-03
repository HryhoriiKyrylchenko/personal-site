using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.Skills.Commands.DeleteSkill;

public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, Result>
{
    private readonly ISkillRepository _repository;
    private readonly ILearningSkillRepository _learningSkillRepository;
    private readonly IUserSkillRepository _userSkillRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSkillCommandHandler> _logger;

    public DeleteSkillCommandHandler(
        ISkillRepository repository, 
        ILearningSkillRepository learningSkillRepository, 
        IUserSkillRepository userSkillRepository,
        IUnitOfWork unitOfWork, 
        ILogger<DeleteSkillCommandHandler> logger)
    {
        _repository = repository;
        _learningSkillRepository = learningSkillRepository;
        _userSkillRepository = userSkillRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var skill = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (skill == null)
            {
                _logger.LogWarning("Skill not found.");
                return Result.Failure("Skill not found.");
            }
            
            var learningSkill = await _learningSkillRepository.GetByIdAsync(request.Id, cancellationToken);
            if (learningSkill != null)
            {
                learningSkill.IsDeleted = true;
                await _learningSkillRepository.UpdateAsync(learningSkill, cancellationToken);
            }
            
            var userSkill = await _userSkillRepository.GetByIdAsync(request.Id, cancellationToken);
            if (userSkill != null)
            {
                userSkill.IsDeleted = true;
                await _userSkillRepository.UpdateAsync(userSkill, cancellationToken);
            }

            skill.IsDeleted = true;
            await _repository.UpdateAsync(skill, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting skill.");
            return Result.Failure("Error while deleting skill.");
        }
    }
}