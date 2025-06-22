namespace PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;

public class GetResumesQueryValidator : AbstractValidator<GetResumesQuery>
{
    public GetResumesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}