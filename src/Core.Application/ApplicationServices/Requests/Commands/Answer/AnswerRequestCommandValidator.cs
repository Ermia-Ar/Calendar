using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

public class AnswerRequestCommandValidator : AbstractValidator<AnswerRequestCommandRequest>
{
    public AnswerRequestCommandValidator()
    {
        RuleFor(x => x.IsAccepted);
    }
}

