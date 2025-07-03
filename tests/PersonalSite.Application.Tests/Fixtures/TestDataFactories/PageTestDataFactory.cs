using PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;
using PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Entities.Pages;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public class PageTestDataFactory
{
    public static Page CreatePage(Guid? id = null, string key = "page-key")
    {
        var pageId = id ?? Guid.NewGuid();
        var language1 = CommonTestDataFactory.CreateLanguage();
        var language2 = CommonTestDataFactory.CreateLanguage("pl");
        var page = new Page
        {
            Id = pageId,
            Key = key,
            Translations = [
                new PageTranslation
                {
                    Id = Guid.NewGuid(),
                    PageId = pageId,
                    LanguageId = language1.Id,
                    Language = language1,
                    Title = "Title 1",
                    Description = "Description 1",
                    MetaTitle = "MetaTitle 1",
                    MetaDescription = "MetaDescription 1",
                    OgImage = "ogimage1.png"
                },
                new PageTranslation
                {
                    Id = Guid.NewGuid(),
                    PageId = pageId,
                    LanguageId = language2.Id,
                    Language = language2,
                    Title = "Title 2",
                    Description = "Description 2",
                    MetaTitle = "MetaTitle 2",
                    MetaDescription = "MetaDescription 2",
                    OgImage = "ogimage2.png"
                }
            ]
        };

        return page;
    }

    public static PageDto MapToDto(Page page)
    {
        return new PageDto
        {
            Id = page.Id,
            Key = page.Key,
            Data = page.Translations.First().Data,
            Title = page.Translations.First().Title,
            Description = page.Translations.First().Description,
            MetaTitle = page.Translations.First().MetaTitle,
            MetaDescription = page.Translations.First().MetaDescription,
            OgImage = page.Translations.First().OgImage
        };
    }

    public static PageAdminDto MapToAdminDto(Page page)
    {
        var dto = new PageAdminDto
        {
            Id = page.Id,
            Key = page.Key,
            Translations = []
        };

        foreach (var t in page.Translations)
        {
            dto.Translations.Add(new PageTranslationDto
            {
                Id = t.Id,
                LanguageCode = t.Language.Code,
                PageId = t.PageId,
                Title = t.Title,
                Data = t.Data,
                Description = t.Description,
                MetaTitle = t.MetaTitle,
                MetaDescription = t.MetaDescription,
                OgImage = t.OgImage
            });
        }

        return dto;
    }

    public static PageTranslationDto CreatePageTranslationDto(
        string languageCode = "en",
        string title = "Test",
        string description = "Test description",
        string metaTitle = "Test",
        string metaDescription = "Test",
        string ogImage = "image.png",
        Dictionary<string, string>? data = null)
    {
        return new PageTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = languageCode,
            Title = title,
            Description = description,
            MetaTitle = metaTitle,
            MetaDescription = metaDescription,
            OgImage = ogImage,
            Data = data ?? new Dictionary<string, string>()
        };
    }
    
    
    public static CreatePageCommand CreateCreatePageCommand(string key = "about") => new(
        key,
        [
            new PageTranslationDto
            {
                LanguageCode = "en",
                Title = "About",
                Description = "About page",
                MetaTitle = "About Meta",
                MetaDescription = "About Meta Description",
                OgImage = "og-about.png",
                Data = new Dictionary<string, string> { ["content"] = "Welcome!" }
            }
        ]
    );

    public static UpdatePageCommand CreateUpdatePageCommand(Page page)
    {
        return new UpdatePageCommand(
            page.Id,
            page.Key,
            page.Translations.Select(t => new PageTranslationDto
            {
                LanguageCode = t.Language.Code,
                Title = "Updated Title",
                Data = t.Data,
                Description = t.Description,
                MetaTitle = t.MetaTitle,
                MetaDescription = t.MetaDescription,
                OgImage = t.OgImage
            }).ToList());
    }

    public static UpdatePageCommand CreateUpdatePageCommand(Guid id, string key, List<PageTranslationDto> translations)
    {
        return new UpdatePageCommand(
            id,
            key,
            translations);       
    }
}