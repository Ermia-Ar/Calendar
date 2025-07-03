using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;

public class SubmitProjectRequestCommandValidator : AbstractValidator<SubmitProjectRequestCommandRequest>
{
    public SubmitProjectRequestCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .Must(x => Guid.TryParse(x, out var result));

        RuleForEach(x => x.MemberIds)
            .Must(x => Guid.TryParse(x, out var result));
    }
}
