using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Requests.Exceptions;

public class NotFoundMemberException :MamrpBaseNotFoundException
{
    public NotFoundMemberException(string message)
        : base("",message)
    {
    }
}
