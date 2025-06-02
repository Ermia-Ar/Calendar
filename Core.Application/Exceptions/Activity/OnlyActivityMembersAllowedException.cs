using Share.Abstract;

namespace Core.Application.Exceptions.Activity;

public class OnlyActivityMembersAllowedException : BadRequestException
{
    public OnlyActivityMembersAllowedException() : base("Only the members of this activity has access to this section.")
    {

    }
}
