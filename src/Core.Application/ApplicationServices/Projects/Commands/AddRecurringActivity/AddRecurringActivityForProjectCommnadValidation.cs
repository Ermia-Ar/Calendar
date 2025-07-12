using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.AddRecurringActivity;

public sealed class AddRecurringActivityForProjectCommnadValidation : AbstractValidator<AddRecurringActivityForProjectCommnadRequest>
{
    public AddRecurringActivityForProjectCommnadValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must be at most 100 characters.");

        RuleFor(x => x.Description);

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.MinValue).WithMessage("StartDate must be a valid date.")
            .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate.");

        RuleFor(x => x.EndDate)
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

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid activity category.");

        RuleFor(x => x.Recurrence)
            .IsInEnum().
            WithMessage("Invalid recurrence type.");
    }
}
