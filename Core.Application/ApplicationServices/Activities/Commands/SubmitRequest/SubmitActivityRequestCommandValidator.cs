using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public class SubmitActivityRequestCommandValidator
    : AbstractValidator<SubmitActivityRequestCommandRequest>
{
    public SubmitActivityRequestCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .Must(x => Guid.TryParse(x, out var result));

        RuleFor(x => x.Message);

        RuleForEach(x => x.MemberIds)
            .Must(x => Guid.TryParse(x, out var result));
    }
}
