using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;

public class GetUserSkillByIdHandler : IRequestHandler<GetUserSkillByIdQuery, Result<UserSkillDto>>
{
    private readonly IUserSkillRepository _repository;
    private readonly ILogger<GetUserSkillByIdHandler> _logger;
    private readonly IMapper<UserSkill, UserSkillDto> _mapper;
    
    public GetUserSkillByIdHandler(
        IUserSkillRepository repository,
        ILogger<GetUserSkillByIdHandler> logger,
        IMapper<UserSkill, UserSkillDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<UserSkillDto>> Handle(GetUserSkillByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("User skill not found.");   
                return Result<UserSkillDto>.Failure("User skill not found.");
            }
            var dto = _mapper.MapToDto(entity);
            return Result<UserSkillDto>.Success(dto);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting user skill by id.");
            return Result<UserSkillDto>.Failure("Error getting user skill by id.");     
        }
    }
}