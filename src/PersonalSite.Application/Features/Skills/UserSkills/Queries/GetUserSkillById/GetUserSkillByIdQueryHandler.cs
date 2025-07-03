using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;

public class GetUserSkillByIdQueryHandler : IRequestHandler<GetUserSkillByIdQuery, Result<UserSkillAdminDto>>
{
    private readonly IUserSkillRepository _repository;
    private readonly ILogger<GetUserSkillByIdQueryHandler> _logger;
    private readonly IAdminMapper<UserSkill, UserSkillAdminDto> _mapper;
    
    public GetUserSkillByIdQueryHandler(
        IUserSkillRepository repository,
        ILogger<GetUserSkillByIdQueryHandler> logger,
        IAdminMapper<UserSkill, UserSkillAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<UserSkillAdminDto>> Handle(GetUserSkillByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("User skill not found.");   
                return Result<UserSkillAdminDto>.Failure("User skill not found.");
            }
            var dto = _mapper.MapToAdminDto(entity);
            return Result<UserSkillAdminDto>.Success(dto);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting user skill by id.");
            return Result<UserSkillAdminDto>.Failure("Error getting user skill by id.");     
        }
    }
}