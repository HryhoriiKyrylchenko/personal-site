using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;

public class GetSkillCategoriesQueryHandler : IRequestHandler<GetSkillCategoriesQuery, Result<List<SkillCategoryAdminDto>>>
{
    private readonly ISkillCategoryRepository _repository;
    private readonly ILogger<GetSkillCategoriesQueryHandler> _logger;
    private readonly IAdminMapper<SkillCategory, SkillCategoryAdminDto> _mapper;

    public GetSkillCategoriesQueryHandler(
        ISkillCategoryRepository repository, 
        ILogger<GetSkillCategoriesQueryHandler> logger,
        IAdminMapper<SkillCategory, SkillCategoryAdminDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;       
    }

    public async Task<Result<List<SkillCategoryAdminDto>>> Handle(GetSkillCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var skillCategories = await _repository.GetFilteredAsync(
                request.KeyFilter,
                request.MinDisplayOrder,
                request.MaxDisplayOrder,
                cancellationToken);

            if (skillCategories.Count == 0)
            {
                _logger.LogWarning("Skill categories not found.");
                return Result<List<SkillCategoryAdminDto>>.Failure("Skill categories not found.");
            }
            
            var items = _mapper.MapToAdminDtoList(skillCategories);
            
            return Result<List<SkillCategoryAdminDto>>.Success(items);           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving skill categories.");
            return Result<List<SkillCategoryAdminDto>>.Failure("Error while retrieving skill categories.");       
        }
    }
}