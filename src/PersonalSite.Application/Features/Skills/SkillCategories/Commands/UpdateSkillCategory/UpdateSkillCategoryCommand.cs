using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.UpdateSkillCategory;

public record UpdateSkillCategoryCommand(
    Guid Id,
    string Key,
    short DisplayOrder,
    List<SkillCategoryTranslationDto> Translations
) : IRequest<Result>;