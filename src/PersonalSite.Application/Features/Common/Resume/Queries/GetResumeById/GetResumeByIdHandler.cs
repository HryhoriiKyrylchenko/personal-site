using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;

public class GetResumeByIdHandler : IRequestHandler<GetResumeByIdQuery, Result<ResumeDto>>
{
    private readonly IResumeRepository _repository;
    private readonly ILogger<GetResumeByIdHandler> _logger;   
    private readonly IMapper<Domain.Entities.Common.Resume, ResumeDto> _mapper;   
    
    public GetResumeByIdHandler(
        IResumeRepository repository,
        ILogger<GetResumeByIdHandler> logger,
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