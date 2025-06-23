namespace PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;

public class GetLogEntriesQueryValidator : AbstractValidator<GetLogEntriesQuery>
{
    public GetLogEntriesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}