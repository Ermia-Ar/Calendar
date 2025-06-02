using Share.Abstract;

namespace Core.Application.Exceptions.Comment
{
    public class OnlyCommentCreatorAllowedException : BadRequestException
    {
        public OnlyCommentCreatorAllowedException() : base("Only the creator of the Comment is allowed.")
        {

        }
    }
}
