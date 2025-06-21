namespace PersonalSite.Application.Features.Skills.Skills.Commands.DeleteSkill;

public record DeleteSkillCommand(Guid Id) : IRequest<Result>;