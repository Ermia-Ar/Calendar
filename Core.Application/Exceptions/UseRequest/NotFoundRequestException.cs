using Share.Abstract;

namespace Core.Application.Exceptions.UseRequest;

public class NotFoundRequestException : NotFoundException
{
    public NotFoundRequestException() : base("Request not found")
    {

    }
}
