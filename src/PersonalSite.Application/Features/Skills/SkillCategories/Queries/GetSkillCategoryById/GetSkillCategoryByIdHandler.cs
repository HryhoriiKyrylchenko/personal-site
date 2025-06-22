using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;

public class GetSkillCategoryByIdHandler : IRequestHandler<GetSkillCategoryByIdQuery, Result<SkillCategoryDto>>
{
    private readonly ISkillCategoryRepository _repository;
    private readonly ILogger<GetSkillCategoryByIdHandler> _logger;
    private readonly IMapper<SkillCategory, SkillCategoryDto> _mapper;
    
    public GetSkillCategoryByIdHandler(
        ISkillCategoryRepository repository,
        ILogger<GetSkillCategoryByIdHandler> logger,
        IMapper<SkillCategory, SkillCategoryDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<SkillCategoryDto>> Handle(GetSkillCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetWithTranslationsByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Skill category not found.");   
                return Result<SkillCategoryDto>.Failure("Skill category not found.");
            }
            var dto = _mapper.MapToDto(entity);
            return Result<SkillCategoryDto>.Success(dto);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting skill category by id.");
            return Result<SkillCategoryDto>.Failure("Error getting skill category by id.");      
        }
    }
}