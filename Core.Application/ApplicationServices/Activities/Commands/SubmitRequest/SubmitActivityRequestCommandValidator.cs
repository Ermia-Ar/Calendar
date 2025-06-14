using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public class SubmitActivityRequestCommandValidator
    : AbstractValidator<SubmitActivityRequestCommandRequest>
{
    public SubmitActivityRequestCommandValidator()
    {

    }
}
