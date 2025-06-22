using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;

public record GetSkillCategoryByIdQuery(Guid Id) : IRequest<Result<SkillCategoryDto>>;
