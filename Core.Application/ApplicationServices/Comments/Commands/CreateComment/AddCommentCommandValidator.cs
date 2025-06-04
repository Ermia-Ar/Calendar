using FluentValidation;

namespace Core.Application.ApplicationServices.Comments.Commands.CreateComment;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommandRequest>
{
    public AddCommentCommandValidator()
    {
        
    }
}
