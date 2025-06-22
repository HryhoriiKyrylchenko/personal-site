using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;

public class GetLearningSkillsQueryHandler : IRequestHandler<GetLearningSkillsQuery, Result<List<LearningSkillAdminDto>>>
{
    private readonly ILearningSkillRepository _repository;
    private readonly ILogger<GetLearningSkillsQueryHandler> _logger;
    private readonly IAdminMapper<LearningSkill, LearningSkillAdminDto> _learningSkillMapper;

    public GetLearningSkillsQueryHandler(
        ILearningSkillRepository repository, 
        ILogger<GetLearningSkillsQueryHandler> logger,
        IAdminMapper<LearningSkill, LearningSkillAdminDto> learningSkillMapper)
    {
        _repository = repository;
        _logger = logger;
        _learningSkillMapper = learningSkillMapper;   
    }

    public async Task<Result<List<LearningSkillAdminDto>>> Handle(GetLearningSkillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var learningSkills = await _repository.ListAsync(cancellationToken);

            if (learningSkills.Count == 0)
            {
                _logger.LogWarning("Learning skills not found.");
                return Result<List<LearningSkillAdminDto>>.Failure("Learning skills not found.");
            }

            var items = _learningSkillMapper.MapToAdminDtoList(learningSkills);
            
            return Result<List<LearningSkillAdminDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get learning skills.");
            return Result<List<LearningSkillAdminDto>>.Failure("Failed to get learning skills.");
        }
    }
}