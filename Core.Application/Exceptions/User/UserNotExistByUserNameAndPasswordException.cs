using Share.Abstract;

namespace Core.Application.Exceptions.User;

public class UserNotExistByUserNameAndPasswordException : BadRequestException
{
    public UserNotExistByUserNameAndPasswordException() : base("Wrong username or password entered.")
    {

    }
}
