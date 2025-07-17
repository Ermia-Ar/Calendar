using SharedKernel.Helper;

namespace Core.Application.InternalServices.Auth.Dto;

public class GetUserByIdDto : IResponse
{
	public Guid Id { get; set; }

	public string UserName { get; set; }

	public string FirstName { get; set; }

	public string FamilyName { get; set; }
	
	public TimeSpan DefaultNotificationBefore { set; get; }
}
