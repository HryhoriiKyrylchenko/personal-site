using PersonalSite.Application.Features.Skills.Common.Dtos;

namespace PersonalSite.Application.Services.Skills;

public interface IProjectSkillService : ICrudService<ProjectSkillDto, ProjectSkillAddRequest, ProjectSkillUpdateRequest>
{
    Task<IReadOnlyList<ProjectSkillDto>> GetAllByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}