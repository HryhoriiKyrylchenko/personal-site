using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;

public class GetLearningSkillByIdQueryHandler : IRequestHandler<GetLearningSkillByIdQuery, Result<LearningSkillAdminDto>>
{
    private readonly ILearningSkillRepository _repository;
    private readonly ILogger<GetLearningSkillByIdQueryHandler> _logger;
    private readonly IAdminMapper<LearningSkill, LearningSkillAdminDto> _learningSkillMapper;
    
    public GetLearningSkillByIdQueryHandler(
        ILearningSkillRepository repository,
        ILogger<GetLearningSkillByIdQueryHandler> logger,
        IAdminMapper<LearningSkill, LearningSkillAdminDto> learningSkillMapper)
    {
        _repository = repository;
        _logger = logger;
        _learningSkillMapper = learningSkillMapper;   
    }


    public async Task<Result<LearningSkillAdminDto>> Handle(GetLearningSkillByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetWithFullDataByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Learning skill not found.");   
                return Result<LearningSkillAdminDto>.Failure("Learning skill not found.");
            }
            var dto = _learningSkillMapper.MapToAdminDto(entity);
            return Result<LearningSkillAdminDto>.Success(dto);           
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting learning skill by id.");
            return Result<LearningSkillAdminDto>.Failure("Error getting learning skill by id.");      
        }
    }
}