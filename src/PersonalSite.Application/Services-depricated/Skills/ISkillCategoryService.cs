using PersonalSite.Application.Features.Skills.Common.Dtos;

namespace PersonalSite.Application.Services.Skills;

public interface ISkillCategoryService : ICrudService<SkillCategoryDto, SkillCategoryAddRequest, SkillCategoryUpdateRequest>
{
}