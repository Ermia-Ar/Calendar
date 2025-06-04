using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.UserRequests.Exceptions;

public class NotFoundMemberException :MamrpBaseNotFoundException
{
    public NotFoundMemberException(string message)
        : base("",message)
    {
    }
}
