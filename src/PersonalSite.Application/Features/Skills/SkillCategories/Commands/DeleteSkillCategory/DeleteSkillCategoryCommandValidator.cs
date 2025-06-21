namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;

public class DeleteSkillCategoryCommandValidator : AbstractValidator<DeleteSkillCategoryCommand>
{
    public DeleteSkillCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}