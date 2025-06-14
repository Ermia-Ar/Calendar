using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.Exiting;

public class ExitingActivityCommandValidator : AbstractValidator<ExitingActivityCommandRequest>
{
    public ExitingActivityCommandValidator()
    {
        
    }
}