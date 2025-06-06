namespace PersonalSite.Application.Services.Skills;

public interface IUserSkillService : ICrudService<UserSkillDto, UserSkillAddRequest, UserSkillUpdateRequest>
{
    Task<List<SkillDto>> GetAllAsSkillAsync(CancellationToken cancellationToken = default);
    Task<List<UserSkillDto>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default);
}