namespace PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;

public class GetSkillByIdQueryValidator : AbstractValidator<GetSkillByIdQuery>
{
    public GetSkillByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");       
    }
}