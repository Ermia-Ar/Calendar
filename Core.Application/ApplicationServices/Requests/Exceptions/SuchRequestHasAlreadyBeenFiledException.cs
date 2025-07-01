using Core.Domain.Entities.Requests;
using SharedKernel.Exceptions;
using System.Security.Policy;

namespace Core.Application.ApplicationServices.Requests.Exceptions;

public class SuchRequestHasAlreadyBeenFiledException :MamrpBaseBadRequestException
{
	public SuchRequestHasAlreadyBeenFiledException(string userName)
        : base("", $"Such a request has already been filed for {userName}")
    {
    }
}
