using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;

public class GetSkillCategoryByIdQueryHandler : IRequestHandler<GetSkillCategoryByIdQuery, Result<SkillCategoryAdminDto>>
{
    private readonly ISkillCategoryRepository _repository;
    private readonly ILogger<GetSkillCategoryByIdQueryHandler> _logger;
    private readonly IAdminMapper<SkillCategory, SkillCategoryAdminDto> _mapper;
    
    public GetSkillCategoryByIdQueryHandler(
        ISkillCategoryRepository repository,
        ILogger<GetSkillCategoryByIdQueryHandler> logger,
        IAdminMapper<SkillCategory, SkillCategoryAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Result<SkillCategoryAdminDto>> Handle(GetSkillCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetWithTranslationsByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Skill category not found.");   
                return Result<SkillCategoryAdminDto>.Failure("Skill category not found.");
            }
            var dto = _mapper.MapToAdminDto(entity);
            return Result<SkillCategoryAdminDto>.Success(dto);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting skill category by id.");
            return Result<SkillCategoryAdminDto>.Failure("Error getting skill category by id.");      
        }
    }
}