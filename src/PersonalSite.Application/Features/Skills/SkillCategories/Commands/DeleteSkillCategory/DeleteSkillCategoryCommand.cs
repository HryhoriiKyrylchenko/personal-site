using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;

public record DeleteSkillCategoryCommand(Guid Id) : IRequest<Result>;