using PersonalSite.Application.Features.Common.Common.Dtos;

namespace PersonalSite.Application.Services.Translations;

public interface ILanguageService : ICrudService<LanguageDto, LanguageAddRequest, LanguageUpdateRequest>
{
    Task<bool> IsSupportedAsync(string code, CancellationToken cancellationToken = default);
}