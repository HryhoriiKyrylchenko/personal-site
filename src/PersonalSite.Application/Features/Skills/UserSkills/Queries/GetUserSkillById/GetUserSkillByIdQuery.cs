using PersonalSite.Application.Features.Skills.UserSkills.Dtos;

namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;

public record GetUserSkillByIdQuery(Guid Id) : IRequest<Result<UserSkillDto>>;