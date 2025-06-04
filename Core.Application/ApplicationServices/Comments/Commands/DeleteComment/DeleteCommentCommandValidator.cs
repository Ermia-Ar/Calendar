using FluentValidation;

namespace Core.Application.ApplicationServices.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommandRequest>
    {
        public DeleteCommentCommandValidator()
        {
            
        }
    }
}
