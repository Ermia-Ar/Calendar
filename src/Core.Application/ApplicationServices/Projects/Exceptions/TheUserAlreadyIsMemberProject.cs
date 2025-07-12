using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Projects.Exceptions;

public class TheUserAlreadyIsMemberProject : MamrpBaseBadRequestException
{
    public TheUserAlreadyIsMemberProject(Guid userId)
        : base("", $"The {userId} Has Already Is a Member of project")
    {
    }
}
