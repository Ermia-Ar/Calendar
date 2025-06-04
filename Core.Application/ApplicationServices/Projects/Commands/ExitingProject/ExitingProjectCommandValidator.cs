using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.ExitingProject;

public class ExitingProjectCommandValidator : AbstractValidator<ExitingProjectCommandRequest>
{
    public ExitingProjectCommandValidator()
    {
        
    }
}
