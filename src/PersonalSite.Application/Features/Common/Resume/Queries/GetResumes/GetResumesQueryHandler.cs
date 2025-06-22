using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;

public class GetResumesQueryHandler : IRequestHandler<GetResumesQuery, PaginatedResult<ResumeDto>>
{
    private readonly IResumeRepository _repository;
    private readonly ILogger<GetResumesQueryHandler> _logger;
    private readonly IMapper<Domain.Entities.Common.Resume, ResumeDto> _mapper;

    public GetResumesQueryHandler(
        IResumeRepository repository,
        ILogger<GetResumesQueryHandler> logger,
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
            var resumes = await _repository.GetFilteredAsync(
                request.Page, 
                request.PageSize, 
                request.IsActive, 
                cancellationToken);

            if (resumes.IsFailure || resumes.Value == null)
            {
                _logger.LogWarning("Resumes not found");
                return PaginatedResult<ResumeDto>.Failure("Resumes not found");
            }
        
            var items = _mapper.MapToDtoList(resumes.Value);

            return PaginatedResult<ResumeDto>.Success(items, resumes.PageNumber, resumes.PageSize, resumes.TotalCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting resumes.");
            return PaginatedResult<ResumeDto>.Failure("An error occurred while getting the resumes.");      
        }
    }
}