using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;

public sealed class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommandRequest>
{
    public UpdateNotificationCommandValidator()
    {
		RuleFor(x => x.ActivityId)
		   .Must(x => Guid.TryParse(x, out var result));
	}
}
