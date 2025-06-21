namespace PersonalSite.Application.Features.Skills.Skills.Commands.UpdateSkill;

public record UpdateSkillCommand(
    Guid Id, 
    Guid CategoryId, 
    string Key,
    List<SkillTranslationDto> Translations) : IRequest<Result>;