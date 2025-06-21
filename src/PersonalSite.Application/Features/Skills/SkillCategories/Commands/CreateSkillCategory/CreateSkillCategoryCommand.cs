namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;

public record CreateSkillCategoryCommand(
    string Key,
    short DisplayOrder,
    List<SkillCategoryTranslationDto> Translations
) : IRequest<Result<Guid>>;