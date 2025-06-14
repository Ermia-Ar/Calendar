using FluentValidation;

namespace Core.Application.ApplicationServices.Comments.Commands.Remove
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommandRequest>
    {
        public DeleteCommentCommandValidator()
        {
            
        }
    }
}
