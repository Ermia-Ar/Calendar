using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Activities.Exceptions;

public class OnlyActivityMembersAllowedException : MamrpBaseBadRequestException
{
    public OnlyActivityMembersAllowedException() 
        : base("","Only the members of this activity has access to this section.")
    {

    }
}
