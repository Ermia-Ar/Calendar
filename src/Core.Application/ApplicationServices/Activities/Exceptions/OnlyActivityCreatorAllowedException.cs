using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Activities.Exceptions;

public class OnlyActivityCreatorAllowedException : MamrpBaseBadRequestException
{
    public OnlyActivityCreatorAllowedException()
        : base("","Only the creator of the activity is allowed.")
    {
    }
}
