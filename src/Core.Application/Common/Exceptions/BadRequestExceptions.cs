using SharedKernel.Exceptions;

namespace Core.Application.Common.Exceptions;

public class BadRequestExceptions : MamrpBaseBadRequestException
{
    public BadRequestExceptions(string message) : base("2001", message)
    {

    }
}
