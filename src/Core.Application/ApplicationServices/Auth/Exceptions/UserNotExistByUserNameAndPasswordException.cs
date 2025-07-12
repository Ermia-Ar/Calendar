using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Auth.Exceptions;

public class UserNotExistByUserNameAndPasswordException : MamrpBaseBadRequestException
{
    public UserNotExistByUserNameAndPasswordException() : base("", "Wrong username or password entered.")
    {
    }
}