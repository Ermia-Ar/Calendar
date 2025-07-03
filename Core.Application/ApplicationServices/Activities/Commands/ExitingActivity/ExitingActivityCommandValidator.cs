using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;

public class ExitingActivityCommandValidator : AbstractValidator<ExitingActivityCommandRequest>
{
    public ExitingActivityCommandValidator()
    {
		RuleFor(x => x.ActivityId)
		   .Must(x => Guid.TryParse(x, out var result));
	}
}