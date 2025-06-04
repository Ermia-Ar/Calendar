using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.UserRequests.Exceptions;

public class NotFoundRequestException : MamrpBaseNotFoundException
{
    public NotFoundRequestException() : base("","Request not found")
    {

    }
}
