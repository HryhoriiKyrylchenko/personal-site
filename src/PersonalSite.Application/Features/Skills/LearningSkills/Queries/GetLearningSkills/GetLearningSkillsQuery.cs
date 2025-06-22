using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;

public record GetLearningSkillsQuery(
    Guid? SkillId = null,
    LearningStatus? Status = null
) : IRequest<Result<List<LearningSkillAdminDto>>>;
