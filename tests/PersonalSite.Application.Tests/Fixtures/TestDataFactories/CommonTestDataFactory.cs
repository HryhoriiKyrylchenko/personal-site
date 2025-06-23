using System.Globalization;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public static class CommonTestDataFactory
{
    public static Language CreateLanguage(string code = "en")
    {
        return new Language
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(code)
        };   
    }
    
    public static LanguageDto MapToDto(Language language)
    {
        return new LanguageDto
        {
            Id = language.Id,
            Code = language.Code,
            Name = language.Name
        };
    }
}