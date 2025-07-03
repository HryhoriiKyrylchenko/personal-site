using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;

public record CreateLearningSkillCommand(
    Guid SkillId,
    LearningStatus LearningStatus,
    short DisplayOrder
) : IRequest<Result<Guid>>;