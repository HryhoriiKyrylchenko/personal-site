namespace PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;

public class GetUserSkillByIdQueryValidator : AbstractValidator<GetUserSkillByIdQuery>
{
    public GetUserSkillByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("UserSkill ID is required.");       
    }
}