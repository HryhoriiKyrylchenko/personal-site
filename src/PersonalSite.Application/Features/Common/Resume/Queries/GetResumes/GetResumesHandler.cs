namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;

public class GetResumesHandler : IRequestHandler<GetResumesQuery, PaginatedResult<ResumeDto>>
{
    private readonly IResumeRepository _repository;
    private readonly ILogger<GetResumesHandler> _logger;
    private readonly IMapper<Domain.Entities.Common.Resume, ResumeDto> _mapper;

    public GetResumesHandler(
        IResumeRepository repository,
        ILogger<GetResumesHandler> logger,
        IMapper<Domain.Entities.Common.Resume, ResumeDto> mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;  
    }

    public async Task<PaginatedResult<ResumeDto>> Handle(GetResumesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetQueryable().AsNoTracking();

            var total = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderByDescending(x => x.UploadedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        
            var items = _mapper.MapToDtoList(entities);

            return PaginatedResult<ResumeDto>.Success(items, total, request.Page, request.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting resumes.");
            return PaginatedResult<ResumeDto>.Failure("An error occurred while getting the resumes.");      
        }
    }
}