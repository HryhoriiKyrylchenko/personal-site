using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;

public class GetLearningSkillByIdHandler : IRequestHandler<GetLearningSkillByIdQuery, Result<LearningSkillDto>>
{
    private readonly ILearningSkillRepository _repository;
    private readonly ILogger<GetLearningSkillByIdHandler> _logger;
    private readonly IAdminMapper<LearningSkill, LearningSkillDto> _learningSkillMapper;
    
    public GetLearningSkillByIdHandler(
        ILearningSkillRepository repository,
        ILogger<GetLearningSkillByIdHandler> logger,
        IAdminMapper<LearningSkill, LearningSkillDto> learningSkillMapper)
    {
        _repository = repository;
        _logger = logger;
        _learningSkillMapper = learningSkillMapper;   
    }


    public async Task<Result<LearningSkillDto>> Handle(GetLearningSkillByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetWithFullDataByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Learning skill not found.");   
                return Result<LearningSkillDto>.Failure("Learning skill not found.");
            }
            var dto = _learningSkillMapper.MapToAdminDto(entity);
            return Result<LearningSkillDto>.Success(dto);           
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting learning skill by id.");
            return Result<LearningSkillDto>.Failure("Error getting learning skill by id.");      
        }
    }
}