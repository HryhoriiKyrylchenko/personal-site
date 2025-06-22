using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;

public record GetUserSkillByIdQuery(Guid Id) : IRequest<Result<UserSkillAdminDto>>;