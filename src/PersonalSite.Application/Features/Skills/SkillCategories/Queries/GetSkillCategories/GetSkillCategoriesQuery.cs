using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;

public record GetSkillCategoriesQuery(
    string? KeyFilter = null,
    short? MinDisplayOrder = null,
    short? MaxDisplayOrder = null
) : IRequest<Result<List<SkillCategoryAdminDto>>>;
