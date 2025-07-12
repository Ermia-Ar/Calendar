using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Projects.Exceptions;

public class InvalidProjectIdException : MamrpBaseBadRequestException
{
    public InvalidProjectIdException()
        : base("", "ProjectId is not valid !")
    {
    }
}
