using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Comments.Exceptions;

public class OnlyCommentCreatorAllowedException : MamrpBaseBadRequestException
{
    public OnlyCommentCreatorAllowedException() 
        : base("","Only the creator of the Comment is allowed.")
    {

    }
}
