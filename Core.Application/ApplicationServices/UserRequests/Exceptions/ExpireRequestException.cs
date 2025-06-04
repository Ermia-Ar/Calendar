using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.UserRequests.Exceptions;

public class ExpireRequestException : MamrpBaseBadRequestException
{
    public ExpireRequestException() : base("","This request has expired !")
    {

    }
}
