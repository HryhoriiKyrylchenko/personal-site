using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;

public record GetLearningSkillByIdQuery(Guid Id) : IRequest<Result<LearningSkillDto>>;
