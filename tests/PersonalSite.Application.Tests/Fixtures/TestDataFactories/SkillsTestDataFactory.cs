using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public class SkillsTestDataFactory
{
    public static Skill CreateSkillWithTranslationsAndCategory()
    {
        var language = CommonTestDataFactory.CreateLanguage();
        
        var category = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Key = "backend",
            DisplayOrder = 1,
            Translations = new List<SkillCategoryTranslation>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Language = language,
                    SkillCategoryId = Guid.NewGuid(),
                    Name = "Backend",
                    Description = "Backend dev"
                }
            }
        };

        var skill = new Skill
        {
            Id = Guid.NewGuid(),
            Key = "dotnet",
            Category = category,
            CategoryId = category.Id,
            Translations = new List<SkillTranslation>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    SkillId = Guid.NewGuid(),
                    Name = ".NET",
                    Description = "Framework",
                    Language = language
                }
            }
        };

        return skill;
    }

    public static SkillAdminDto MapToAdminDto(Skill skill)
    {
        return new SkillAdminDto
        {
            Id = skill.Id,
            Key = skill.Key,
            Category = new SkillCategoryAdminDto
            {
                Id = skill.Category.Id,
                Key = skill.Category.Key,
                DisplayOrder = skill.Category.DisplayOrder,
                Translations = skill.Category.Translations.Select(t => new SkillCategoryTranslationDto
                {
                    Id = t.Id,
                    LanguageCode = t.Language.Code,
                    SkillCategoryId = t.SkillCategoryId,
                    Name = t.Name,
                    Description = t.Description
                }).ToList()
            },
            Translations = skill.Translations.Select(t => new SkillTranslationDto
            {
                Id = t.Id,
                SkillId = t.SkillId,
                LanguageCode = t.Language.Code,
                Name = t.Name,
                Description = t.Description
            }).ToList()
        };
    }
    
    public static Skill CreateSkill(string key = "csharp")
    {
        var language = CommonTestDataFactory.CreateLanguage();
        var category = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Key = "backend",
            DisplayOrder = 1,
            Translations = []
        };

        return new Skill
        {
            Id = Guid.NewGuid(),
            Key = key,
            CategoryId = category.Id,
            Category = category,
            Translations = new List<SkillTranslation>
            {
                new SkillTranslation
                {
                    Id = Guid.NewGuid(),
                    LanguageId = language.Id,
                    Language = language,
                    Name = "C#",
                    Description = "C# Programming Language"
                }
            }
        };
    }

    public static SkillTranslationDto CreateTranslationDto(string lang = "en")
    {
        return new SkillTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = lang,
            Name = "C#",
            Description = "C# Language"
        };
    }

    public static SkillTranslation CreateTranslation(Skill skill, Language language)
    {
        return new SkillTranslation
        {
            Id = Guid.NewGuid(),
            SkillId = skill.Id,
            Skill = skill,
            LanguageId = language.Id,
            Language = language,
            Name = "C#",
            Description = "C# Language"
        };
    }

    public static SkillCategory CreateSkillCategory()
    {
        var language = CommonTestDataFactory.CreateLanguage();
        return new SkillCategory
        {
            Id = Guid.NewGuid(),
            Key = "backend",
            DisplayOrder = 1,
            Translations =
            [
                new SkillCategoryTranslation
                {
                    Id = Guid.NewGuid(),
                    Language = language,
                    LanguageId = language.Id,
                    SkillCategoryId = Guid.NewGuid(),
                    Name = "Backend",
                    Description = "Backend dev"
                }
            ]
        };
    }
    
    public static SkillCategoryAdminDto MapToAdminDto(SkillCategory category)
    {
        return new SkillCategoryAdminDto
        {
            Id = category.Id,
            Key = category.Key,
            DisplayOrder = category.DisplayOrder,
            Translations = category.Translations.Select(t => new SkillCategoryTranslationDto
            {
                Id = t.Id,
                SkillCategoryId = t.SkillCategoryId,
                LanguageCode = t.Language.Code,
                Name = t.Name,
                Description = t.Description
            }).ToList()
        };
    }
    
    public static LearningSkill CreateLearningSkillWithSkill()
    {
        var skill = CreateSkillWithTranslationsAndCategory(); 
        return new LearningSkill
        {
            Id = Guid.NewGuid(),
            Skill = skill,
            SkillId = skill.Id,
            LearningStatus = LearningStatus.InProgress, 
            DisplayOrder = 1
        };
    }

    public static LearningSkillAdminDto MapToLearningSkillAdminDto(LearningSkill entity)
    {
        return new LearningSkillAdminDto
        {
            Id = entity.Id,
            Skill = MapToAdminDto(entity.Skill),
            LearningStatus = entity.LearningStatus,
            DisplayOrder = entity.DisplayOrder
        };
    }
}