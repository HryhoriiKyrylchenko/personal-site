namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;

public class GetResumesHandler : IRequestHandler<GetResumesQuery, PaginatedResult<ResumeDto>>
{
    private readonly IResumeRepository _repository;

    public GetResumesHandler(IResumeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResult<ResumeDto>> Handle(GetResumesQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(x => x.UploadedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        var items = ResumeMapper.MapToDtoList(entities);

        return PaginatedResult<ResumeDto>.Success(items, total, request.Page, request.PageSize);
    }
}