using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Requests.Exceptions;

public class NotFoundRequestException : MamrpBaseNotFoundException
{
    public NotFoundRequestException() : base("","Request not found")
    {

    }
}
