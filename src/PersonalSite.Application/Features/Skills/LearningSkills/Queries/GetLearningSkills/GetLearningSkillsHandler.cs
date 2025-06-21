namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;

public class GetLearningSkillsHandler : IRequestHandler<GetLearningSkillsQuery, Result<List<LearningSkillAdminDto>>>
{
    private readonly ILearningSkillRepository _repository;
    private readonly ILogger<GetLearningSkillsHandler> _logger;

    public GetLearningSkillsHandler(ILearningSkillRepository repository, ILogger<GetLearningSkillsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<LearningSkillAdminDto>>> Handle(GetLearningSkillsQuery request, CancellationToken cancellationToken)
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

            if (request.Status.HasValue)
                query = query.Where(x => x.LearningStatus == request.Status.Value);

            var entities = await query
                .OrderBy(s => s.Skill.Category.Key)
                .ToListAsync(cancellationToken);

            var items = LearningSkillMapper.MapToAdminDtoList(entities);
            
            return Result<List<LearningSkillAdminDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get learning skills.");
            return Result<List<LearningSkillAdminDto>>.Failure("Failed to get learning skills.");
        }
    }
}