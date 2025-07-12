using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public class RemoveMemberOfProjectCommandValidator : AbstractValidator<RemoveMemberOfProjectCommandRequest>
{
    public RemoveMemberOfProjectCommandValidator()
    {

    }
}