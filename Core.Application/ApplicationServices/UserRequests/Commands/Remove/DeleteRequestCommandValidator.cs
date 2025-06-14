using FluentValidation;

namespace Core.Application.ApplicationServices.UserRequests.Commands.Remove;

public class DeleteRequestCommandValidator : AbstractValidator<DeleteRequestCommandRequest>
{
    public DeleteRequestCommandValidator()
    {
        
    }
}
