using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Auth.Exceptions;

public class NotFoundUserIdException : MamrpBaseNotFoundException
{
    public NotFoundUserIdException(string userId)
        : base("", $"user Id {userId} does not exist !")
    {
    }
}
