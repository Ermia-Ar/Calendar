using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity
{
    public class AddSubActivityCommandValidator : AbstractValidator<AddSubActivityCommandRequest>
    {
        public AddSubActivityCommandValidator()
        {
			RuleFor(x => x.Title)
			.NotNull()
			.MinimumLength(3);

			RuleFor(x => x.Description);

			RuleFor(x => x.Duration);

			RuleFor(x => x.NotificationBefore)
				.Custom((notification, context) =>
				{
					var startDate = context.InstanceToValidate.StartDate;
					if (startDate - notification < DateTime.UtcNow)
					{
						context.AddFailure("NotificationBefore"
							, "Notification time must be set before the activity starts and must be in the future.");
					}
				});

			RuleFor(x => x.Type)
				.Must(x => (int)x >= 0 && (int)x <= 1);

			RuleFor(x => x.StartDate)
				.Must(x => x >= DateTime.UtcNow);

		}
	}
}
