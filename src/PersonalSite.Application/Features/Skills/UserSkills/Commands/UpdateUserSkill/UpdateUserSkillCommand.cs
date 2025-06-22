using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.UpdateUserSkill;

public record UpdateUserSkillCommand(
    Guid Id,
    short Proficiency
) : IRequest<Result>;