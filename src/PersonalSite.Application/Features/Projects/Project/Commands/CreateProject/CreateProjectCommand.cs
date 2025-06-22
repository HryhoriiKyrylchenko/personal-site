using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;

public record CreateProjectCommand : IRequest<Result<Guid>>
{
    public string Slug { get; init; } = string.Empty;
    public string CoverImage { get; init; } = string.Empty;
    public string DemoUrl { get; init; } = string.Empty;
    public string RepoUrl { get; init; } = string.Empty;
    public List<ProjectTranslationDto> Translations { get; init; } = new();
    public List<Guid> SkillIds { get; init; } = new();
}