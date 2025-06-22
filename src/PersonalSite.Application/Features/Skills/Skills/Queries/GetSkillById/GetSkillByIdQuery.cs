using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;

public record GetSkillByIdQuery(Guid Id) : IRequest<Result<SkillDto>>;
