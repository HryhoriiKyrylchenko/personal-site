using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;

public class CreateUserSkillCommandHandler : IRequestHandler<CreateUserSkillCommand, Result<Guid>>
{
    private readonly IUserSkillRepository _repository;
    private readonly ILearningSkillRepository _learningSkillRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserSkillCommandHandler> _logger;

    public CreateUserSkillCommandHandler(
        IUserSkillRepository repository,
        ILearningSkillRepository learningSkillRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateUserSkillCommandHandler> logger)
    {
        _repository = repository;
        _learningSkillRepository = learningSkillRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateUserSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingSkill = await _repository.GetBySkillIdAsync(request.SkillId, cancellationToken);
            
            if (existingSkill is { IsDeleted: false })
            {
                _logger.LogWarning($"User skill with skill Id {request.SkillId} already exists.");
                return Result<Guid>.Failure("User skill with skill Id already exists.");           
            }
            
            var existingLearningSkill = await _learningSkillRepository.GetByIdAsync(request.SkillId, cancellationToken);
            if (existingLearningSkill != null)
            {
                _learningSkillRepository.Remove(existingLearningSkill);
            }

            if (existingSkill is { IsDeleted: true })
            {
                existingSkill.IsDeleted = false;
                await _repository.UpdateAsync(existingSkill, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                return Result<Guid>.Success(existingSkill.Id);
            }
            
            var entity = new UserSkill
            {
                Id = Guid.NewGuid(),
                SkillId = request.SkillId,
                Proficiency = request.Proficiency,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user skill.");
            return Result<Guid>.Failure("Failed to create user skill.");
        }
    }
}