using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.DeleteUserSkill;

public record DeleteUserSkillCommand(Guid Id) : IRequest<Result>;