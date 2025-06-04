using FluentValidation;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.DeleteActivity
{
    public record class DeleteActivityCommandRequest(
        string Id
        ) : IRequest;

    public  class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommandRequest>
    {
        public DeleteActivityCommandValidator()
        {
            
        }
    }
}
