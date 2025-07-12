using FluentValidation;
using System.Drawing.Text;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommandRequest>
{
    public AddProjectCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .MinimumLength(3);

        RuleFor(x => x.Description);

        RuleFor(x => x.StartDate)
            .Must(x => x >= DateTime.UtcNow);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");
    }


}