using Core.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Calendar.Api.Base
{
    public class AppControllerBase
    {
        public ActionResult NewResult<T>(Response<T> response)
        {
            // there is no list here
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult(string.Empty, response);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(response);
                case HttpStatusCode.NoContent:
                    return new NoContentResult();
                default:
                    return new BadRequestObjectResult(response);
            }
        }
    }
}
