using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.Remove;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommandRequest>
{
    public DeleteProjectCommandValidator()
    {

    }
}
