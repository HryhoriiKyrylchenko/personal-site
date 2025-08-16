using System.Globalization;
using PersonalSite.Application.Features.Common.Language.Commands.CreateLanguage;
using PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;
using PersonalSite.Application.Features.Common.Resume.Commands.UpdateResume;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.UpdateSocialMediaLink;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
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

    public static CreateLanguageCommand CreateCreateLanguageCommand(string code = "en", string name = "English")
    {
        return new CreateLanguageCommand(code, name);
    }

    public static UpdateLanguageCommand CreateUpdateLanguageCommand(Guid id = default, string code = "en",
        string name = "English")
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        return new UpdateLanguageCommand(id, code, name);   
    }
    
    public static Resume CreateResume(Guid? id = null, string? fileUrl = null, string? fileName = null, bool isActive = true)
    {
        return new Resume
        {
            Id = id ?? Guid.NewGuid(),
            FileUrl = fileUrl ?? "https://example.com/resume.pdf",
            FileName = fileName ?? "resume.pdf",
            UploadedAt = DateTime.UtcNow,
            IsActive = isActive
        };
    }

    public static ResumeDto MapToResumeDto(Resume entity)
    {
        return new ResumeDto
        {
            Id = entity.Id,
            FileUrl = entity.FileUrl,
            FileName = entity.FileName,
            UploadedAt = entity.UploadedAt,
            IsActive = entity.IsActive
        };
    }

    public static CreateResumeCommand CreateCreateResumeCommand(string fileUrl = "https://example.com/resume.pdf",
        string fileName = "resume.pdf", bool isActive = true)
    {
        return new CreateResumeCommand(fileUrl, fileName, isActive);
    }

    public static UpdateResumeCommand CreateUpdateResumeCommand(
        Guid id = default, string fileUrl = "https://example.com/resume.pdf",
        string fileName = "resume.pdf", bool isActive = true)
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        return new UpdateResumeCommand(id, fileUrl, fileName, isActive);  
    }
    
    public static SocialMediaLink CreateSocialMediaLink(
        Guid? id = null,
        string platform = "Twitter",
        string url = "https://twitter.com/example",
        short displayOrder = 1,
        bool isActive = true) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Platform = platform,
        Url = url,
        DisplayOrder = displayOrder,
        IsActive = isActive
    };

    public static SocialMediaLinkDto MapToDto(SocialMediaLink entity) => new()
    {
        Id = entity.Id,
        Platform = entity.Platform,
        Url = entity.Url,
        DisplayOrder = entity.DisplayOrder,
        IsActive = entity.IsActive
    };

    public static CreateSocialMediaLinkCommand CreateCreateSocialMediaLinkCommand(
        string platform = "Facebook",
        string url = "https://facebook.com/example",
        int displayOrder = 1,
        bool isActive = true
    )
    {
        return new CreateSocialMediaLinkCommand(platform, url, displayOrder, isActive);
    }

    public static UpdateSocialMediaLinkCommand CreateUpdateSocialMediaLinkCommand(
        Guid id = default,
        string platform = "Facebook",
        string url = "https://facebook.com/example",
        int displayOrder = 1,
        bool isActive = true
    )
    {
        if (id == Guid.Empty) id = Guid.NewGuid();
        return new UpdateSocialMediaLinkCommand(id, platform, url, displayOrder, isActive);
    }
}