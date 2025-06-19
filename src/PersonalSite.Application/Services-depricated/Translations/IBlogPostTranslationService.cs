using PersonalSite.Application.Features.Translations.Common.Dtos;

namespace PersonalSite.Application.Services.Translations;

public interface IBlogPostTranslationService : ICrudService<BlogPostTranslationDto, BlogPostTranslationAddRequest, BlogPostTranslationUpdateRequest>
{
}