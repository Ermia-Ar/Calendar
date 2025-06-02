using Share.Abstract;

namespace Core.Application.Exceptions.User;

public class BadRegisterRequestException : BadRequestException
{
    public BadRegisterRequestException(string message) : base(message)
    {

    }
}
