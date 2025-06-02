using Share.Abstract;

namespace Core.Application.Exceptions.Project
{
    public class OnlyProjectCreatorAllowedException : BadRequestException
    {
        public OnlyProjectCreatorAllowedException()
            : base("Only the creator of the project is allowed.")
        {
        }
    }

}
