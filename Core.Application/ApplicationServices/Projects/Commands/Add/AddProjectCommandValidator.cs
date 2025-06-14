using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommandRequest>
{
    public AddProjectCommandValidator()
    {
        
    }
}