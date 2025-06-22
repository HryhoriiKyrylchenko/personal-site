using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;

public record GetSkillCategoryByIdQuery(Guid Id) : IRequest<Result<SkillCategoryAdminDto>>;
