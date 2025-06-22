using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;

public record CreateSkillCommand(
    Guid CategoryId, 
    string Key,
    List<SkillTranslationDto> Translations) : IRequest<Result<Guid>>;