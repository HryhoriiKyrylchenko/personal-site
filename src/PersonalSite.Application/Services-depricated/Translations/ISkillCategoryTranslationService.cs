using PersonalSite.Application.Features.Translations.Common.Dtos;

namespace PersonalSite.Application.Services.Translations;

public interface ISkillCategoryTranslationService : 
    ICrudService<SkillCategoryTranslationDto, SkillCategoryTranslationAddRequest, SkillCategoryTranslationUpdateRequest>
{
}