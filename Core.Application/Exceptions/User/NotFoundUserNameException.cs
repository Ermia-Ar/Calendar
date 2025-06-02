using Share.Abstract;

namespace Core.Application.Exceptions.User
{
    public class NotFoundUserNameException : NotFoundException
    {
        public NotFoundUserNameException(string userName)
            : base($"user name {userName} does not exist !")
        {
        }
    }

}
