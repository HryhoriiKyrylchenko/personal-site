namespace PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;

public class GetSkillCategoryByIdQueryValidator : AbstractValidator<GetSkillCategoryByIdQuery>
{
    public GetSkillCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");       
    }
}