using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Projects;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public static class ProjectTestDataFactory
{
    public static Project CreateProject(Guid? id = null, string? lanuageCode = null)
    {
        var projectId = id ?? Guid.NewGuid();
        var skillCategoryId = Guid.NewGuid();
        var skillId = Guid.NewGuid();

        var language = lanuageCode == null 
            ? CommonTestDataFactory.CreateLanguage() 
            : CommonTestDataFactory.CreateLanguage(code: lanuageCode);
        
        var project = new Project
        {
            Id = projectId,
            Slug = "sample-project",
            CoverImage = "cover.png",
            DemoUrl = "https://demo.example.com",
            RepoUrl = "https://github.com/example/project",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            Translations = new List<ProjectTranslation>
            {
                new ProjectTranslation
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    Language = language,
                    LanguageId = language.Id,
                    Title = "Sample Project",
                    ShortDescription = "A test project",
                    DescriptionSections = new Dictionary<string, string>
                    {
                        { "section1", "This is section 1" }
                    },
                    MetaTitle = "Meta Title",
                    MetaDescription = "Meta Description",
                    OgImage = "ogimage.png"
                }
            },
            ProjectSkills = new List<ProjectSkill>
            {
                new ProjectSkill
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    Skill = new Skill
                    {
                        Id = skillId,
                        Key = "csharp",
                        Category = new SkillCategory
                        {
                            Id = skillCategoryId,
                            Key = "backend",
                            DisplayOrder = 1,
                            Translations = new List<SkillCategoryTranslation>
                            {
                                new SkillCategoryTranslation
                                {
                                    Id = Guid.NewGuid(),
                                    Language = language,
                                    LanguageId = language.Id,
                                    SkillCategoryId = skillCategoryId,
                                    Name = "Backend",
                                    Description = "Backend skills"
                                }
                            }
                        },
                        Translations = new List<SkillTranslation>
                        {
                            new SkillTranslation
                            {
                                Id = Guid.NewGuid(),
                                Language = language,
                                LanguageId = language.Id,
                                SkillId = skillId,
                                Name = "C#",
                                Description = "C# programming language"
                            }
                        }
                    }
                }
            }
        };

        return project;
    }

    public static ProjectAdminDto MapToAdminDto(Project project)
    {
        return new ProjectAdminDto
        {
            Id = project.Id,
            Slug = project.Slug,
            CoverImage = project.CoverImage,
            DemoUrl = project.DemoUrl,
            RepoUrl = project.RepoUrl,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Translations = project.Translations.Select(t => new ProjectTranslationDto
            {
                Id = t.Id,
                LanguageCode = t.Language.Code,
                ProjectId = t.ProjectId,
                Title = t.Title,
                ShortDescription = t.ShortDescription,
                DescriptionSections = t.DescriptionSections,
                MetaTitle = t.MetaTitle,
                MetaDescription = t.MetaDescription,
                OgImage = t.OgImage
            }).ToList(),
            Skills = project.ProjectSkills.Select(ps => new ProjectSkillAdminDto
            {
                Id = ps.Id,
                ProjectId = ps.ProjectId,
                Skill = new SkillAdminDto
                {
                    Id = ps.Skill.Id,
                    Key = ps.Skill.Key,
                    Category = new SkillCategoryAdminDto
                    {
                        Id = ps.Skill.Category.Id,
                        Key = ps.Skill.Category.Key,
                        DisplayOrder = ps.Skill.Category.DisplayOrder,
                        Translations = ps.Skill.Category.Translations.Select(ct => new SkillCategoryTranslationDto
                        {
                            Id = ct.Id,
                            LanguageCode = ct.Language.Code,
                            SkillCategoryId = ct.SkillCategoryId,
                            Name = ct.Name,
                            Description = ct.Description
                        }).ToList()
                    },
                    Translations = ps.Skill.Translations.Select(st => new SkillTranslationDto
                    {
                        Id = st.Id,
                        LanguageCode = st.Language.Code,
                        SkillId = st.SkillId,
                        Name = st.Name,
                        Description = st.Description
                    }).ToList()
                }
            }).ToList()
        };
    }

    public static ProjectTranslationDto CreateTranslationDto(
        Guid? id,
        Guid? projectId,
        string code = "en",
        string title = "Title",
        string shortDescription = "Short description",
        Dictionary<string, string>? descriptionSections = null,
        string metaTitle = "Title",
        string metaDescription = "Description",
        string ogImage = "image.png")
    {
        return new ProjectTranslationDto
        {
            Id = id ?? Guid.NewGuid(),
            LanguageCode = code,
            ProjectId = projectId ?? Guid.NewGuid(),
            Title = title,
            ShortDescription = shortDescription,
            DescriptionSections = descriptionSections ?? [],
            MetaTitle = metaTitle,
            MetaDescription = metaDescription,
            OgImage = ogImage       
        };
    }

    public static CreateProjectCommand CreateCreateProjectCommand(List<ProjectTranslationDto>? translations,
        List<Guid>? skillIds, string slug = "sample-project") => new()
    {
        Slug = slug,
        Translations = translations ?? new List<ProjectTranslationDto>(),
        SkillIds = skillIds ?? new List<Guid>()       
    };
    
    public static (
        UpdateProjectCommand Command,
        Project Project,
        Language Language,
        Guid SkillId) CreateValidUpdateProjectCommandAndEntities()
    {
        var projectId = Guid.NewGuid();
        var skillId = Guid.NewGuid();
        var language = CommonTestDataFactory.CreateLanguage();

        var translationDto = ProjectTestDataFactory.CreateTranslationDto(
            id: null,
            projectId: projectId,
            code: language.Code,
            title: "Title",
            shortDescription: "Short desc",
            descriptionSections: new(),
            metaTitle: "Meta",
            metaDescription: "Meta desc",
            ogImage: "og.png"
        );

        var command = new UpdateProjectCommand
        {
            Id = projectId,
            Slug = "updated-slug",
            CoverImage = "new-image.png",
            DemoUrl = "demo.com",
            RepoUrl = "repo.com",
            SkillIds = [skillId],
            Translations = [translationDto]
        };

        var project = ProjectTestDataFactory.CreateProject(id: projectId, lanuageCode: language.Code);
        project.Slug = "old-slug";
        project.ProjectSkills.Clear();
        project.Translations.Clear();

        return (command, project, language, skillId);
    }
}
