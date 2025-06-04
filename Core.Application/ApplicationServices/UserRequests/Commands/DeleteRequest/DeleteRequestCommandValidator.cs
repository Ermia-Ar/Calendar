using FluentValidation;

namespace Core.Application.ApplicationServices.UserRequests.Commands.DeleteRequest;

public class DeleteRequestCommandValidator : AbstractValidator<DeleteRequestCommandRequest>
{
    public DeleteRequestCommandValidator()
    {
        
    }
}
