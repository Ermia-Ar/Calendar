using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.AddRecurring;

public sealed class AddRecurringActivityCommandValidation : AbstractValidator<AddRecurringActivityCommandRequest>
{
    public AddRecurringActivityCommandValidation()
    {
		RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(100)
			.WithMessage("Title must be at most 100 characters.");

		RuleFor(x => x.Description);

		RuleFor(x => x.StartDate)
			.GreaterThan(DateTime.MinValue).WithMessage("StartDate must be a valid date.")
			.LessThan(x => x.ToDate).WithMessage("StartDate must be before EndDate.");

		RuleFor(x => x.ToDate)
			.GreaterThan(x => x.StartDate)
			.WithMessage("EndDate must be after StartDate.");

		RuleFor(x => x.Interval)
			.GreaterThan(0)
			.WithMessage("Interval must be greater than 0.");

		RuleFor(x => x.Duration)
			.Must(d => d == null || d.Value.TotalMinutes > 0)
			.WithMessage("Duration must be greater than 0 minutes if specified.");

		RuleFor(x => x.NotificationBefore)
			.Custom((notification, context) =>
			{
				var startDate = context.InstanceToValidate.StartDate;
				if (startDate - notification <= DateTime.UtcNow)
				{
					context.AddFailure("NotificationBefore"
						, "Notification time must be set before the activity starts and must be in the future.");
				}
			});

		RuleFor(x => x.MemberIds);

		RuleFor(x => x.Type)
			.IsInEnum()
			.WithMessage("Invalid activity category.");
	}
}
