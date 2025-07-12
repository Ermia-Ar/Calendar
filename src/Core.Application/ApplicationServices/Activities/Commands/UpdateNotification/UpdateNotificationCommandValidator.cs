using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;

public sealed class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommandRequest>
{
    public UpdateNotificationCommandValidator()
    {
	}
}
