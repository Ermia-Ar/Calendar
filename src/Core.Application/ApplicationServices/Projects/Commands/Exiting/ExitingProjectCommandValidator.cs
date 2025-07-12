using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

public class ExitingProjectCommandValidator : AbstractValidator<ExitingProjectCommandRequest>
{
    public ExitingProjectCommandValidator()
    {

    }
}
