using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;

public record CreateUserSkillCommand(
    Guid SkillId,
    short Proficiency
) : IRequest<Result<Guid>>;