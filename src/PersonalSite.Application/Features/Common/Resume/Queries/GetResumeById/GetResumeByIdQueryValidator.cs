namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;

public class GetResumeByIdQueryValidator : AbstractValidator<GetResumeByIdQuery>
{
    public GetResumeByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Resume ID is required.");       
    }
}