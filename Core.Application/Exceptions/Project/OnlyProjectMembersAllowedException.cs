using Share.Abstract;

namespace Core.Application.Exceptions.Project;

public class OnlyProjectMembersAllowedException : BadRequestException
{
    public OnlyProjectMembersAllowedException()
        : base("Only the members of this project has access to this section.")
    {

    }
}
