namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;

public class GetLearningSkillByIdQueryValidator : AbstractValidator<GetLearningSkillByIdQuery>
{
    public GetLearningSkillByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");       
    }
}