using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;

public class GetSkillByIdHandler : IRequestHandler<GetSkillByIdQuery, Result<SkillDto>>
{
    private readonly ISkillRepository _skillRepository;
    private readonly ILogger<GetSkillByIdHandler> _logger;
    private readonly IMapper<Skill, SkillDto> _mapper;
    
    public GetSkillByIdHandler(
        ISkillRepository skillRepository,
        ILogger<GetSkillByIdHandler> logger,
        IMapper<Skill, SkillDto> mapper)
    {
        _skillRepository = skillRepository;
        _logger = logger;   
        _mapper = mapper;
    }


    public async Task<Result<SkillDto>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _skillRepository.GetWithTranslationsById(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Skill with ID {Id} not found.", request.Id);
                return Result<SkillDto>.Failure("Skill not found.");
            }
            var dto = _mapper.MapToDto(entity);
            return Result<SkillDto>.Success(dto);       
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting skill by id.");
            return Result<SkillDto>.Failure("Error getting skill by id.");      
        }
    }
}