using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkills;

public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, Result<List<SkillAdminDto>>>
{
    private readonly ISkillRepository _repository;
    private readonly ILogger<GetSkillsQueryHandler> _logger;
    private readonly IAdminMapper<Skill, SkillAdminDto> _skillAdminMapper;

    public GetSkillsQueryHandler(
        ISkillRepository repository, 
        ILogger<GetSkillsQueryHandler> logger,
        IAdminMapper<Skill, SkillAdminDto> skillAdminMapper)
    {
        _repository = repository;
        _logger = logger;
        _skillAdminMapper = skillAdminMapper;   
    }

    public async Task<Result<List<SkillAdminDto>>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var skills = await _repository.GetFilteredAsync(request.CategoryId, request.KeyFilter, cancellationToken);

            var items = _skillAdminMapper.MapToAdminDtoList(skills);
            
            return Result<List<SkillAdminDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving skills with filters.");
            return Result<List<SkillAdminDto>>.Failure("Error while retrieving skills with filters.");       
        }
    }
}