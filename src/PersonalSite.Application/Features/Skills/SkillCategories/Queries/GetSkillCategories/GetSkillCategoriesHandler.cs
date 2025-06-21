namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;

public class GetSkillCategoriesHandler : IRequestHandler<GetSkillCategoriesQuery, Result<List<SkillCategoryAdminDto>>>
{
    private readonly ISkillCategoryRepository _repository;
    private readonly ILogger<GetSkillCategoriesHandler> _logger;

    public GetSkillCategoriesHandler(ISkillCategoryRepository repository, ILogger<GetSkillCategoriesHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<SkillCategoryAdminDto>>> Handle(GetSkillCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetQueryable()
                .Include(sc => sc.Translations)
                    .ThenInclude(t => t.Language)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.KeyFilter))
                query = query.Where(x => x.Key.Contains(request.KeyFilter));

            if (request.MinDisplayOrder.HasValue)
                query = query.Where(x => x.DisplayOrder >= request.MinDisplayOrder.Value);

            if (request.MaxDisplayOrder.HasValue)
                query = query.Where(x => x.DisplayOrder <= request.MaxDisplayOrder.Value);

            var entities = await query.OrderBy(sc => sc.DisplayOrder).ToListAsync(cancellationToken);
            var items = EntityToDtoMapper.MapSkillCategoriesToAdminDtoList(entities);
            return Result<List<SkillCategoryAdminDto>>.Success(items);           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving skill categories.");
            return Result<List<SkillCategoryAdminDto>>.Failure("Error while retrieving skill categories.");       
        }
    }
}