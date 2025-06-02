using Share.Abstract;

namespace Core.Application.Exceptions.Activity
{
    public class OnlyActivityCreatorAllowedException : BadRequestException
    {
        public OnlyActivityCreatorAllowedException()
            : base("Only the creator of the activity is allowed.")
        {
        }
    }

}
