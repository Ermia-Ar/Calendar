using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.Remove;

public class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommandRequest>
{
    public DeleteActivityCommandValidator()
    {

	}
}
