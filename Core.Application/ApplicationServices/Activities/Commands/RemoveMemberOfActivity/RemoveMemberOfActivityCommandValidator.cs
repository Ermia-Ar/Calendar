using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMemberOfActivity;

public class RemoveMemberOfActivityCommandValidator : AbstractValidator<RemoveMemberOfActivityCommandRequest>
{
    public RemoveMemberOfActivityCommandValidator()
    {
        
    }
}
