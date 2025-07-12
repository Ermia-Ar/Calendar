using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Projects.Exceptions;

public class OnlyProjectMembersAllowedException : MamrpBaseBadRequestException
{
    public OnlyProjectMembersAllowedException()
        : base("", "Only the members of this project has access to this section.")
    {

    }
}
