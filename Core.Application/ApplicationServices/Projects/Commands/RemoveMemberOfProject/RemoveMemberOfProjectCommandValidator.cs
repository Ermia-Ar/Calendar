using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMemberOfProject;

public class RemoveMemberOfProjectCommandValidator : AbstractValidator<RemoveMemberOfProjectCommandRequest>
{
    public RemoveMemberOfProjectCommandValidator()
    {
        
    }
}