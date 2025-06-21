namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.UpdateLearningSkill;

public record UpdateLearningSkillCommand(
    Guid Id,
    LearningStatus LearningStatus,
    short DisplayOrder
) : IRequest<Result>;
