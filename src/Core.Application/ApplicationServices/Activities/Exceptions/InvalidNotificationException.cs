using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Activities.Exceptions;

public class InvalidNotificationException : MamrpBaseBadRequestException
{
    public InvalidNotificationException() 
        : base("", "Invalid Notifcation")
    {
        
    }

}

