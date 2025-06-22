using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkills;

public class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, Result<List<UserSkillAdminDto>>>
{
    private readonly IUserSkillRepository _repository;
    private readonly ILogger<GetUserSkillsQueryHandler> _logger;
    private readonly IAdminMapper<UserSkill, UserSkillAdminDto> _mapper;

    public GetUserSkillsQueryHandler(
        IUserSkillRepository repository, 
        ILogger<GetUserSkillsQueryHandler> logger,
        IAdminMapper<UserSkill, UserSkillAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;       
    }

    public async Task<Result<List<UserSkillAdminDto>>> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userSkills = await _repository.GetFilteredAsync(
                request.SkillId,
                request.MinProficiency,
                request.MaxProficiency,
                cancellationToken);

            var items = _mapper.MapToAdminDtoList(userSkills);
            return Result<List<UserSkillAdminDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user skills.");
            return Result<List<UserSkillAdminDto>>.Failure("Error getting user skills.");       
        }
    }
}
