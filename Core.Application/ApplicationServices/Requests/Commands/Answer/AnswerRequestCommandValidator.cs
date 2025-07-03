using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

public class AnswerRequestCommandValidator : AbstractValidator<AnswerRequestCommandRequest>
{
    public AnswerRequestCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .Must(x => Guid.TryParse(x, out var result));

        RuleFor(x => x.IsAccepted);
    }
}

