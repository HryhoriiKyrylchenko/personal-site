using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;

public record GetLearningSkillByIdQuery(Guid Id) : IRequest<Result<LearningSkillAdminDto>>;
