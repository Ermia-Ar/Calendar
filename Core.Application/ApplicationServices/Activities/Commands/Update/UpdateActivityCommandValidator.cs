﻿using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommandRequest>
{
    public UpdateActivityCommandValidator()
    {
		RuleFor(x => x.Id)
			.Must(x => Guid.TryParse(x, out var result));

		RuleFor(x => x.Title)
			.NotNull()
			.MinimumLength(3);

		RuleFor(x => x.Description);

		RuleFor(x => x.Duration);

		RuleFor(x => x.Category)
			.Must(x => (int)x >= 0 && (int)x <= 1);
	}
}
