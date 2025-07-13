using SharedKernel.Exceptions;

namespace Core.Application.ApplicationServices.Requests.Exceptions;

public class InvalidRequestIdException() : MamrpBaseBadRequestException("", "This requestId is invalid !");