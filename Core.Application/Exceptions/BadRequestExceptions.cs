using Share.Abstract;

namespace Core.Application.Exceptions
{
    public class BadRequestExceptions : BadRequestException
    {
        public BadRequestExceptions(string message) : base(message)
        {

        }
    }
}
