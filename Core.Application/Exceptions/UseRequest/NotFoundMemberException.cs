using Share.Abstract;

namespace Core.Application.Exceptions.UseRequest
{
    public class NotFoundMemberException : NotFoundException
    {
        public NotFoundMemberException(string message)
            : base(message)
        {
        }
    }

}
