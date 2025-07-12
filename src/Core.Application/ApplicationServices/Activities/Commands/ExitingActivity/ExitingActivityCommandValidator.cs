using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;

public class ExitingActivityCommandValidator : AbstractValidator<ExitingActivityCommandRequest>
{
    public ExitingActivityCommandValidator()
    {
		
	}
}