using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;

public class UpdateActivityStartDateCommandValidator : AbstractValidator<UpdateActivityStartDateCommandRequest>
{
    public UpdateActivityStartDateCommandValidator()
    {
        RuleFor(x => x.activityId)
            .Must(x => Guid.TryParse(x, out var result));

        RuleFor(x => x.NewStartDate)
            .Must(x => x >= DateTime.Now);
    }
}