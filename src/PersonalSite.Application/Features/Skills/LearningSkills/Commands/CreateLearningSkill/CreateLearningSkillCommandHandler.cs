using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;

public class CreateLearningSkillCommandHandler : IRequestHandler<CreateLearningSkillCommand, Result<Guid>>
{
    private readonly ILearningSkillRepository _repository;
    private readonly IUserSkillRepository _userSkillRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLearningSkillCommandHandler> _logger;

    public CreateLearningSkillCommandHandler(
        ILearningSkillRepository repository,
        IUserSkillRepository userSkillRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateLearningSkillCommandHandler> logger)
    {
        _repository = repository;
        _userSkillRepository = userSkillRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateLearningSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingSkill = await _repository.GetBySkillIdAsync(request.SkillId, cancellationToken);
            var existingUserSkill = await _userSkillRepository.ExistsBySkillIdAsync(
                request.SkillId,
                cancellationToken);
            if (existingSkill is { IsDeleted: false } || existingUserSkill)
            {
                _logger.LogWarning($"Learning skill or User skill with skill id {request.SkillId} already exists.");
                return Result<Guid>.Failure(
                    $"Learning skill or User skill with skill id {request.SkillId} already exists.");
            }

            if (existingSkill is { IsDeleted: true })
            {
                existingSkill.IsDeleted = false;
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(existingSkill.Id);
            }

            var entity = new LearningSkill
            {
                Id = Guid.NewGuid(),
                SkillId = request.SkillId,
                LearningStatus = request.LearningStatus,
                DisplayOrder = request.DisplayOrder
            };

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating learning skill.");
            return Result<Guid>.Failure("Failed to create learning skill.");
        }
    }
}