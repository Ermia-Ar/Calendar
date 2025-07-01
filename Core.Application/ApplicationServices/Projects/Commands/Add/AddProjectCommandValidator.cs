using FluentValidation;
using System.Drawing.Text;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommandRequest>
{
    public AddProjectCommandValidator()
    {
		RuleFor(x => x.Title)
			.NotNull()
			.NotEmpty()
			.MinimumLength(3);

		RuleFor(x => x.Description);

		RuleFor(x => x.Message);

		RuleFor(x => x.StartDate)
			.Must(x => x >= DateTime.Now);

		RuleFor(x => x.EndDate)
			.Custom((end, context) =>
			{
				var startDate = (context.InstanceToValidate).StartDate;
				if (end <= startDate)
				{
					context.AddFailure("EndDate", "End date must be after start date.");
				}
			});
	}

	
}