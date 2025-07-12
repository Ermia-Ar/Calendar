using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Activities.Exceptions;

public class TheUserAlreadyIsMemberActivity : MamrpBaseBadRequestException
{
	public TheUserAlreadyIsMemberActivity(Guid userId)
		: base("", $"The {userId} Has Already Is a Member of activity")
	{
	}
}
