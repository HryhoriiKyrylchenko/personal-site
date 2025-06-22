using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;

public class CreateUserSkillHandler : IRequestHandler<CreateUserSkillCommand, Result<Guid>>
{
    private readonly IUserSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserSkillHandler> _logger;

    public CreateUserSkillHandler(
        IUserSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<CreateUserSkillHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateUserSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingSkill = await _repository.ExistsBySkillIdAsync(request.SkillId, cancellationToken);
            if (existingSkill)
            {
                _logger.LogWarning($"User skill with skill Id {request.SkillId} already exists.");
                return Result<Guid>.Failure("User skill with skill Id already exists.");           
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