using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.DeleteProject;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommandRequest>
{
    public DeleteProjectCommandValidator()
    {
        
    }
}
