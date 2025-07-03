using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;

public record GetSkillByIdQuery(Guid Id) : IRequest<Result<SkillAdminDto>>;
