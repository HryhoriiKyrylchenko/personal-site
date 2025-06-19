using PersonalSite.Application.Features.Translations.Common.Dtos;

namespace PersonalSite.Application.Services.Translations;

public interface ISkillTranslationService : 
    ICrudService<SkillTranslationDto, SkillTranslationAddRequest, SkillTranslationUpdateRequest>
{
}