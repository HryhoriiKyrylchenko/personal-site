using System.Globalization;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Application.Features.Common.Resume.Dtos;
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
    
    public static LogEntry CreateLogEntry(
        Guid? id = null,
        DateTime? timestamp = null,
        string level = "Information",
        string message = "Sample log message",
        string messageTemplate = "{MessageTemplate}",
        string? exception = null,
        string? properties = null,
        string? sourceContext = "DefaultContext")
    {
        return new LogEntry
        {
            Id = id ?? Guid.NewGuid(),
            Timestamp = timestamp ?? DateTime.UtcNow,
            Level = level,
            Message = message,
            MessageTemplate = messageTemplate,
            Exception = exception,
            Properties = properties,
            SourceContext = sourceContext
        };
    }

    public static LogEntryDto MapToDto(LogEntry log)
    {
        return new LogEntryDto
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            Level = log.Level,
            Message = log.Message,
            MessageTemplate = log.MessageTemplate,
            Exception = log.Exception,
            Properties = log.Properties,
            SourceContext = log.SourceContext,
        };
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
    
    public static SocialMediaLink CreateSocialMediaLink(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Platform = "Twitter",
        Url = "https://twitter.com/example",
        DisplayOrder = 1,
        IsActive = true
    };

    public static SocialMediaLinkDto MapToDto(SocialMediaLink entity) => new()
    {
        Id = entity.Id,
        Platform = entity.Platform,
        Url = entity.Url,
        DisplayOrder = entity.DisplayOrder,
        IsActive = entity.IsActive
    };
}