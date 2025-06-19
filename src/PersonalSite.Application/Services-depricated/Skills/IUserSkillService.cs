using PersonalSite.Application.Features.Skills.Common.Dtos;

namespace PersonalSite.Application.Services.Skills;

public interface IUserSkillService : ICrudService<UserSkillDto, UserSkillAddRequest, UserSkillUpdateRequest>
{
}