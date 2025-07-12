using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Activities.Exceptions;

public class InvalidActivityIdException : MamrpBaseBadRequestException
{
	public InvalidActivityIdException()
		: base("", "ProjectId is not valid !")
	{
	}
}
