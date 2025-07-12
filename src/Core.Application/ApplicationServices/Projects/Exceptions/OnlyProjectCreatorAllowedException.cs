using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Projects.Exceptions;

public class OnlyProjectCreatorAllowedException : MamrpBaseBadRequestException
{
    public OnlyProjectCreatorAllowedException()
        : base("", "Only the creator of the project is allowed.")
    {
    }
}

