using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;

public class UpdateActivityStartDateCommandValidator : AbstractValidator<UpdateActivityStartDateCommandRequest>
{
    public UpdateActivityStartDateCommandValidator()
    {
        RuleFor(x => x.NewStartDate)
            .Must(x => x >= DateTime.UtcNow);
    }
}