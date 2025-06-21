namespace PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;

public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.SlugFilter)
            .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.SlugFilter))
            .WithMessage("SlugFilter must be 100 characters or fewer.");
    }
}