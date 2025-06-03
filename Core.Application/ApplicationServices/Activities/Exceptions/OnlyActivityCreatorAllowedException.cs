using Share.Abstract;

namespace Core.Application.ApplicationServices.Activities.Exceptions
{
    public class OnlyActivityCreatorAllowedException : BadRequestException
    {
        public OnlyActivityCreatorAllowedException()
            : base("Only the creator of the activity is allowed.")
        {
        }
    }

}
