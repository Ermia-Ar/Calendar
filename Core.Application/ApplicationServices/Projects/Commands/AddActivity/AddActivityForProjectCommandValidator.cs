using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.AddActivity
{
    public class AddActivityForProjectCommandValidator : AbstractValidator<AddActivityForProjectCommandRequest>
    {
        public AddActivityForProjectCommandValidator()
        {
			RuleFor(x => x.ProjectId)
				.Must(x => Guid.TryParse(x, out var result));

			RuleFor(x => x.Title)
			.NotEmpty()
			.MinimumLength(3);

			RuleFor(x => x.Description);

			RuleFor(x => x.Message);

			RuleFor(x => x.Duration);

			RuleFor(x => x.NotificationBefore)
				.Custom((notification, context) =>
				{
					var startDate = context.InstanceToValidate.StartDate;
					if (startDate - notification < DateTime.Now)
					{
						context.AddFailure("NotificationBefore"
							, "Notification time must be set before the activity starts and must be in the future.");
					}
				});

			RuleFor(x => x.Category)
				.Must(x => (int)x >= 0 && (int)x <= 1);

			RuleFor(x => x.StartDate)
				.Must(x => x >= DateTime.Now);

		}
	}
}
