using FluentValidation;

namespace Core.Application.ApplicationServices.Requests.Commands.Remove;

public class DeleteRequestCommandValidator : AbstractValidator<DeleteRequestCommandRequest>
{
    public DeleteRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out var result));

    }
}
