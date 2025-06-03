using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public class AddActivityCommandValidator : AbstractValidator<AddActivityCommandRequest>
{
    public AddActivityCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(3)
            .NotNull();

        RuleFor(x => x.StartDate)
            .NotNull()
            .GreaterThanOrEqualTo(DateTime.UtcNow - TimeSpan.FromMinutes(3));

        RuleFor(x => x.Category)
            .Must(x => (int)x <= 1);
    }
}
