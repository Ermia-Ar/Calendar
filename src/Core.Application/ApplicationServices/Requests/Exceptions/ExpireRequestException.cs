using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Requests.Exceptions;

public class ExpireRequestException : MamrpBaseBadRequestException
{
    public ExpireRequestException() : base("","This request has expired !")
    {

    }
}
