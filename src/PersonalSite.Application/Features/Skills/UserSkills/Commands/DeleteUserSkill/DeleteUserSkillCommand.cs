namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.DeleteUserSkill;

public record DeleteUserSkillCommand(Guid Id) : IRequest<Result>;