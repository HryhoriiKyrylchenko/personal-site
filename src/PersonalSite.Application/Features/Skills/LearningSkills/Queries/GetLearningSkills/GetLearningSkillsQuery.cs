using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;

public record GetLearningSkillsQuery(
    LearningStatus? LearningStatus = null
    ) : IRequest<Result<List<LearningSkillAdminDto>>>;
