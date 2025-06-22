namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;

public record DeleteSkillCategoryCommand(Guid Id) : IRequest<Result>;