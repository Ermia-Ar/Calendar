using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.AddProject;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommandRequest>
{
    public AddProjectCommandValidator()
    {
        
    }
}