using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;

public record CreateSkillCommand(
    Guid CategoryId, 
    string Key,
    List<SkillTranslationDto> Translations) : IRequest<Result<Guid>>;