using SharedKernel.Exceptions;

namespace Core.Application.Exceptions;

public class BadRequestExceptions : MamrpBaseBadRequestException
{
    public BadRequestExceptions(string message) : base("2001", message)
    {

    }
}
