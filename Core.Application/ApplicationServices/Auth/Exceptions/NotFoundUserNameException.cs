using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Auth.Exceptions;

public class NotFoundUserNameException : MamrpBaseNotFoundException
{
    public NotFoundUserNameException(string userName)
        : base("", $"user name {userName} does not exist !")
    {
    }
}
