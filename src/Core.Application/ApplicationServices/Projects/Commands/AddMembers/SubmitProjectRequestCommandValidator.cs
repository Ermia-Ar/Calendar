using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.AddMembers;

public class SubmitProjectRequestCommandValidator : AbstractValidator<AddMembersToProjectCommandRequest>
{
    public SubmitProjectRequestCommandValidator()
    {

    }
}
