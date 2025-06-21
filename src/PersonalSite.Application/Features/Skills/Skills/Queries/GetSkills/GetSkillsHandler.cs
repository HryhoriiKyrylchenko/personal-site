namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkills;

public class GetSkillsHandler : IRequestHandler<GetSkillsQuery, Result<List<SkillAdminDto>>>
{
    private readonly ISkillRepository _repository;
    private readonly ILogger<GetSkillsHandler> _logger;

    public GetSkillsHandler(ISkillRepository repository, ILogger<GetSkillsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<SkillAdminDto>>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetQueryable()
                .Include(s => s.Category)
                    .ThenInclude(c => c.Translations)
                        .ThenInclude(t => t.Language)
                .Include(s => s.Translations)
                    .ThenInclude(t => t.Language)
                .AsNoTracking();

            if (request.CategoryId.HasValue)
                query = query.Where(s => s.CategoryId == request.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(request.KeyFilter))
                query = query.Where(s => s.Key.Contains(request.KeyFilter));

            var entities = await query
                .OrderBy(s => s.Key)
                .ToListAsync(cancellationToken);

            var items = EntityToDtoMapper.MapSkillsToAdminDtoList(entities);
            
            return Result<List<SkillAdminDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving skills with filters.");
            return Result<List<SkillAdminDto>>.Failure("Error while retrieving skills with filters.");       
        }
    }
}