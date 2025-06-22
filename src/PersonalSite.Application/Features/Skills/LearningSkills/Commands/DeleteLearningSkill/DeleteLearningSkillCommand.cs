namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;

public record DeleteLearningSkillCommand(Guid Id) : IRequest<Result>;