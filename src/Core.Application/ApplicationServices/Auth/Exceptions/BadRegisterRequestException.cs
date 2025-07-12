using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Auth.Exceptions;

public class BadRegisterRequestException : MamrpBaseBadRequestException
{
    public BadRegisterRequestException(string message) 
        : base("",message)
    {

    }
}
