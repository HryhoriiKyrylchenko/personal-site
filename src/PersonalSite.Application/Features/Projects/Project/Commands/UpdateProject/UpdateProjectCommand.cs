using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;

public record UpdateProjectCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = string.Empty;
    public string CoverImage { get; init; } = string.Empty;
    public string DemoUrl { get; init; } = string.Empty;
    public string RepoUrl { get; init; } = string.Empty;
    public List<ProjectTranslationDto> Translations { get; init; } = [];
    public List<Guid> SkillIds { get; init; } = [];
}