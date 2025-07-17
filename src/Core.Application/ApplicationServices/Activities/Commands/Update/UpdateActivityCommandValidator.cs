using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommandRequest>
{
    public UpdateActivityCommandValidator()
    {
		RuleFor(x => x.Title)
			.NotNull()
			.MinimumLength(3);

		RuleFor(x => x.Description);

		RuleFor(x => x.Duration);

		RuleFor(x => x.Type)
			.Must(x => (int)x >= 0 && (int)x <= 1);
	}
}
