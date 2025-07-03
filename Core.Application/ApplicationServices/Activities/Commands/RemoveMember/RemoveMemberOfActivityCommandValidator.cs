using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMember;

public class RemoveMemberOfActivityCommandValidator : AbstractValidator<RemoveMemberOfActivityCommandRequest>
{
    public RemoveMemberOfActivityCommandValidator()
    {
		RuleFor(x => x.ActivityId)
		   .Must(x => Guid.TryParse(x, out var result));


		RuleFor(x => x.UserId)
		   .Must(x => Guid.TryParse(x, out var result));
	}
}
