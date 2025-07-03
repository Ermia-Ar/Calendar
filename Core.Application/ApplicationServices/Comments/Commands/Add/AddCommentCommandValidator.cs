using FluentValidation;

namespace Core.Application.ApplicationServices.Comments.Commands.Add;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommandRequest>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .Must(x => Guid.TryParse(x, out var result));

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
