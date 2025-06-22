using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;

public class GetResumeByIdQueryHandler : IRequestHandler<GetResumeByIdQuery, Result<ResumeDto>>
{
    private readonly IResumeRepository _repository;
    private readonly ILogger<GetResumeByIdQueryHandler> _logger;   
    private readonly IMapper<Domain.Entities.Common.Resume, ResumeDto> _mapper;   
    
    public GetResumeByIdQueryHandler(
        IResumeRepository repository,
        ILogger<GetResumeByIdQueryHandler> logger,
        IMapper<Domain.Entities.Common.Resume, ResumeDto> mapper)
    {
        _repository = repository;
        _logger = logger;   
        _mapper = mapper;   
    }


    public async Task<Result<ResumeDto>> Handle(GetResumeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var resume = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (resume == null)
                return Result<ResumeDto>.Failure("Resume not found.");
            var result = _mapper.MapToDto(resume);
            return Result<ResumeDto>.Success(result);      
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting resume by id.");
            return Result<ResumeDto>.Failure("Error getting resume by id.");      
        }
    }
}