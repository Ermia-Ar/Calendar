using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.Remove;

public class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommandRequest>
{
    public DeleteActivityCommandValidator()
    {

		RuleFor(x => x.Id)
		   .Must(x => Guid.TryParse(x, out var result));
	}
}
