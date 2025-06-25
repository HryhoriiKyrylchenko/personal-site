using System.Globalization;
using PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Commands.PublishBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public static class BlogPostTestDataFactory
{
    public static BlogPost CreateBlogPost(
        Guid? postId = null,
        string? slug = null,
        bool isPublished = false,
        bool isDeleted = false,
        DateTime? createdAt = null,
        DateTime? publishedAt = null,
        List<BlogPostTranslation>? translations = null,
        List<PostTag>? postTags = null)
    {
        var id = postId ?? Guid.NewGuid();

        return new BlogPost
        {
            Id = id,
            Slug = slug ?? $"slug-{id.ToString()[..8]}",
            CoverImage = "cover.jpg",
            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = null,
            IsPublished = isPublished,
            PublishedAt = isPublished ? (publishedAt ?? DateTime.UtcNow) : null,
            IsDeleted = isDeleted,
            Translations = translations ?? 
            [
                CreateBlogPostTranslation(id, CommonTestDataFactory.CreateLanguage()),
                CreateBlogPostTranslation(id, CommonTestDataFactory.CreateLanguage("pl"))
            ],
            PostTags = postTags ?? 
            [
                CreatePostTag(id, CreateBlogPostTag()),
                CreatePostTag(id, CreateBlogPostTag("Web"))
            ]
        };
    }
    
    public static BlogPostTranslation CreateBlogPostTranslation(Guid blogPostId, Language? language = null)
    {
        language ??= CommonTestDataFactory.CreateLanguage();

        return new BlogPostTranslation
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            LanguageId = language.Id,
            Language = language,
            Title = $"Title {language.Code}",
            Excerpt = $"Excerpt {language.Code}",
            Content = $"Content {language.Code}",
            MetaTitle = $"MetaTitle {language.Code}",
            MetaDescription = $"MetaDesc {language.Code}",
            OgImage = $"og_{language.Code}.jpg"
        };
    }
    
    public static PostTag CreatePostTag(Guid blogPostId, BlogPostTag? tag = null)
    {
        tag ??= CreateBlogPostTag();

        return new PostTag
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            BlogPostTagId = tag.Id,
            BlogPostTag = tag
        };
    }
    
    public static BlogPostTag CreateBlogPostTag(string name = "Tech")
    {
        return new BlogPostTag
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }

    public static BlogPostAdminDto MapToAdminDto(BlogPost post)
    {
        return new BlogPostAdminDto
        {
            Id = post.Id,
            Slug = post.Slug,
            CoverImage = post.CoverImage,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            IsPublished = post.IsPublished,
            IsDeleted = post.IsDeleted,
            PublishedAt = post.PublishedAt,
            Translations = post.Translations.Select(MapToTranslationDto).ToList(),
            Tags = post.PostTags.Select(MapToTagDto).ToList()
        };
    }

    public static BlogPostTranslationDto MapToTranslationDto(BlogPostTranslation t)
    {
        return new BlogPostTranslationDto
        {
            Id = t.Id,
            LanguageCode = t.Language.Code,
            BlogPostId = t.BlogPostId,
            Title = t.Title,
            Excerpt = t.Excerpt,
            Content = t.Content,
            MetaTitle = t.MetaTitle,
            MetaDescription = t.MetaDescription,
            OgImage = t.OgImage
        };
    }

    public static BlogPostTagDto MapToTagDto(PostTag pt)
    {
        return new BlogPostTagDto
        {
            Id = pt.BlogPostTagId,
            Name = pt.BlogPostTag.Name
        };
    }
    
    public static CreateBlogPostCommand CreateValidCreateBlogPostCommand(
        string slug = "valid-slug",
        bool isPublished = true,
        List<BlogPostTranslationDto>? translations = null,
        List<BlogPostTagDto>? tags = null)
    {
        return new CreateBlogPostCommand(
            Slug: slug,
            CoverImage: "cover.jpg",
            IsPublished: isPublished,
            Translations: translations ??
            [
                new BlogPostTranslationDto
                {
                    Id = Guid.NewGuid(),
                    LanguageCode = "en",
                    Title = "Title",
                    Excerpt = "Excerpt",
                    Content = "Content",
                    MetaTitle = "MetaTitle",
                    MetaDescription = "MetaDesc",
                    OgImage = "og.jpg"
                }
            ],
            Tags: tags ??
            [
                new BlogPostTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Tech"
                }
            ]
        );
    }
    
    public static BlogPostTranslationDto CreateTranslationDto(
        string code = "en",
        string? title = null,
        string? excerpt = null,
        string? content = null,
        string? metaTitle = null,
        string? metaDescription = null,
        string? ogImage = null)
    {
        return new BlogPostTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = code,
            Title = title ?? $"Title {code}",
            Excerpt = excerpt ?? $"Excerpt {code}",
            Content = content ?? $"Content {code}",
            MetaTitle = metaTitle ?? $"MetaTitle {code}",
            MetaDescription = metaDescription ?? $"MetaDesc {code}",
            OgImage = ogImage ?? $"og_{code}.jpg"
        };
    }
    
    public static BlogPostTagDto CreateTagDto(Guid? id = null, string name = "Tech")
    {
        return new BlogPostTagDto
        {
            Id = id ?? Guid.NewGuid(),
            Name = name
        };
    }
    
    public static BlogPost CreateUnpublishedBlogPost(Guid? id = null)
    {
        return CreateBlogPost(
            postId: id,
            isPublished: false,
            publishedAt: null
        );
    }

    public static BlogPost CreatePublishedBlogPost(Guid? id = null, DateTime? publishedAt = null)
    {
        return CreateBlogPost(
            postId: id,
            isPublished: true,
            publishedAt: publishedAt ?? DateTime.UtcNow
        );
    }
    
    public static PublishBlogPostCommand CreatePublishCommand(Guid id, bool isPublished = true, DateTime? publishDate = null)
    {
        return new PublishBlogPostCommand
        {
            Id = id,
            IsPublished = isPublished,
            PublishDate = publishDate ?? DateTime.UtcNow
        };
    }
    
    public static UpdateBlogPostCommand CreateValidUpdateCommand(
        Guid id = default, 
        string slug = "new-slug", 
        Guid? tagId = null,
        List<BlogPostTranslationDto>? translations = null)
    {
        translations ??= [CreateTranslationDto()];
        
        return new UpdateBlogPostCommand(
            Id: id,
            Slug: slug,
            CoverImage: "new-cover.jpg",
            IsDeleted: true,
            Translations: translations,
            Tags: [CreateTagDto(tagId ?? Guid.NewGuid())]
        );
    }

    public static BlogPostTranslation CreateTranslation(string code, Guid blogPostId)
    {
        var language = CommonTestDataFactory.CreateLanguage(code);
        return new BlogPostTranslation
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            LanguageId = language.Id,
            Language = language,
            Title = $"Title {code}",
            Excerpt = $"Excerpt {code}",
            Content = $"Content {code}",
            MetaTitle = $"Meta {code}",
            MetaDescription = $"Desc {code}",
            OgImage = $"og_{code}.jpg"
        };
    }

    public static BlogPost CreateSimpleBlogPost(Guid? id = null, string slug = "old-slug")
    {
        return new BlogPost
        {
            Id = id ?? Guid.NewGuid(),
            Slug = slug,
            CoverImage = "old-cover.jpg",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };
    }
}