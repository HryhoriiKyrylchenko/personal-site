namespace PersonalSite.Application.Services.Common;

public interface ILanguageService
{
    Task<bool> IsSupportedAsync(string code, CancellationToken cancellationToken = default);
}