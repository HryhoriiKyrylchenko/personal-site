namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkills;

public class GetUserSkillsHandler : IRequestHandler<GetUserSkillsQuery, Result<List<UserSkillAdminDto>>>
{
    private readonly IUserSkillRepository _repository;
    private readonly ILogger<GetUserSkillsHandler> _logger;

    public GetUserSkillsHandler(IUserSkillRepository repository, ILogger<GetUserSkillsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<UserSkillAdminDto>>> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetQueryable()
                .Include(x => x.Skill)
                    .ThenInclude(s => s.Translations.Where(t => !t.Language.IsDeleted))
                        .ThenInclude(t => t.Language)
                .Include(x => x.Skill)
                    .ThenInclude(s => s.Category)
                        .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                            .ThenInclude(t => t.Language)
                .AsNoTracking();

            if (request.SkillId.HasValue)
                query = query.Where(x => x.SkillId == request.SkillId.Value);

            if (request.MinProficiency.HasValue)
                query = query.Where(x => x.Proficiency >= request.MinProficiency.Value);

            if (request.MaxProficiency.HasValue)
                query = query.Where(x => x.Proficiency <= request.MaxProficiency.Value);

            var entities = await query
                .OrderBy(s => s.Skill.Category.Key)
                .ToListAsync(cancellationToken);

            var items = UserSkillMapper.MapToAdminDtoList(entities);
            return Result<List<UserSkillAdminDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user skills.");
            return Result<List<UserSkillAdminDto>>.Failure("Error getting user skills.");       
        }
    }
}
