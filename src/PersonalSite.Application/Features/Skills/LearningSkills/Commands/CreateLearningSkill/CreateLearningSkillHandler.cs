namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;

public class CreateLearningSkillHandler : IRequestHandler<CreateLearningSkillCommand, Result<Guid>>
{
    private readonly ILearningSkillRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLearningSkillHandler> _logger;

    public CreateLearningSkillHandler(
        ILearningSkillRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<CreateLearningSkillHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateLearningSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingSkill = await _repository.ExistsBySkillIdAsync(request.SkillId, cancellationToken);
            if (existingSkill)
            {
                _logger.LogWarning($"Learning skill with skill id {request.SkillId} already exists.");
                return Result<Guid>.Failure($"Learning skill with skill id {request.SkillId} already exists.");           
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