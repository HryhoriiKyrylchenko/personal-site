using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;

public class GetSkillByIdQueryHandler : IRequestHandler<GetSkillByIdQuery, Result<SkillAdminDto>>
{
    private readonly ISkillRepository _skillRepository;
    private readonly ILogger<GetSkillByIdQueryHandler> _logger;
    private readonly IAdminMapper<Skill, SkillAdminDto> _mapper;
    
    public GetSkillByIdQueryHandler(
        ISkillRepository skillRepository,
        ILogger<GetSkillByIdQueryHandler> logger,
        IAdminMapper<Skill, SkillAdminDto> mapper)
    {
        _skillRepository = skillRepository;
        _logger = logger;   
        _mapper = mapper;
    }


    public async Task<Result<SkillAdminDto>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _skillRepository.GetWithTranslationsById(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Skill with ID {Id} not found.", request.Id);
                return Result<SkillAdminDto>.Failure("Skill not found.");
            }
            var dto = _mapper.MapToAdminDto(entity);
            return Result<SkillAdminDto>.Success(dto);       
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting skill by id.");
            return Result<SkillAdminDto>.Failure("Error getting skill by id.");      
        }
    }
}