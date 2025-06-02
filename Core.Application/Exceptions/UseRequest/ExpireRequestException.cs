using Share.Abstract;

namespace Core.Application.Exceptions.UseRequest;

public class ExpireRequestException : BadRequestException
{
    public ExpireRequestException() : base("This request has expired !")
    {

    }
}
