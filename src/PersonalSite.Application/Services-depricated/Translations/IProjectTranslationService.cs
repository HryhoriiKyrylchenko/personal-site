using PersonalSite.Application.Features.Translations.Common.Dtos;

namespace PersonalSite.Application.Services.Translations;

public interface IProjectTranslationService : 
    ICrudService<ProjectTranslationDto, ProjectTranslationAddRequest, ProjectTranslationUpdateRequest>
{
}