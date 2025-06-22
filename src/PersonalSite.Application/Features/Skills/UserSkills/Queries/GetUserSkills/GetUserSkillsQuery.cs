using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkills;

public record GetUserSkillsQuery(
    Guid? SkillId = null,
    short? MinProficiency = null,
    short? MaxProficiency = null
) : IRequest<Result<List<UserSkillAdminDto>>>;