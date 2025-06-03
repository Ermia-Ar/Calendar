using Share.Abstract;

namespace Core.Application.ApplicationServices.Activities.Exceptions;

public class OnlyActivityMembersAllowedException : BadRequestException
{
    public OnlyActivityMembersAllowedException() : base("Only the members of this activity has access to this section.")
    {

    }
}
