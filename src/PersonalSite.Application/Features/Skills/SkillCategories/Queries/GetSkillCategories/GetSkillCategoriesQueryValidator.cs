namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;

public class GetSkillCategoriesQueryValidator : AbstractValidator<GetSkillCategoriesQuery>
{
    public GetSkillCategoriesQueryValidator()
    {
        RuleFor(x => x.KeyFilter)
            .MaximumLength(50)
            .WithMessage("Key filter must be 50 characters or fewer.")
            .When(x => !string.IsNullOrWhiteSpace(x.KeyFilter));

        RuleFor(x => x)
            .Must(x => !x.MinDisplayOrder.HasValue || !x.MaxDisplayOrder.HasValue || x.MinDisplayOrder <= x.MaxDisplayOrder)
            .WithMessage("MinDisplayOrder cannot be greater than MaxDisplayOrder.");
    }
}