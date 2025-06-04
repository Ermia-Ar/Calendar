using FluentValidation;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommandRequest>
{
    public UpdateActivityCommandValidator()
    {
        
    }
}
