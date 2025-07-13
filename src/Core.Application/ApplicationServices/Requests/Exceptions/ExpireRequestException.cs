using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Requests.Exceptions;

public class ExpireRequestException() : MamrpBaseBadRequestException("", "This request has expired !");