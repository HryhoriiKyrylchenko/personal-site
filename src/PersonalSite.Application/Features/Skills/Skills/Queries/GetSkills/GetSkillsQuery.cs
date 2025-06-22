using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkills;

public record GetSkillsQuery(
    Guid? CategoryId = null,
    string? KeyFilter = null
) : IRequest<Result<List<SkillAdminDto>>>;