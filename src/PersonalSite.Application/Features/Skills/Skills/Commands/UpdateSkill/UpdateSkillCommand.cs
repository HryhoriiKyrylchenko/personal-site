using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.Skills.Commands.UpdateSkill;

public record UpdateSkillCommand(
    Guid Id, 
    Guid CategoryId, 
    string Key,
    List<SkillTranslationDto> Translations) : IRequest<Result>;