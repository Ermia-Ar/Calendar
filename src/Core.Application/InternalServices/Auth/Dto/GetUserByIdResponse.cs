using Newtonsoft.Json;
using SharedKernel.Helper;

namespace Core.Application.InternalServices.Auth.Dto;

public class GetUserByIdResponse : IResponse
{
	public Guid Id { get; set; }

	public string UserName { get; set; }

	public string FirstName { get; set; }

	public string FamilyName { get; set; }
}
