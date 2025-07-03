using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.UpdateSkillCategory;

public record UpdateSkillCategoryCommand(
    Guid Id,
    string Key,
    short DisplayOrder,
    List<SkillCategoryTranslationDto> Translations
) : IRequest<Result>;