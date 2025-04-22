using Core.Domain.Resources;

namespace Core.Domain.Shared
{
    public class ResponseHandler
    {

        public Response<T> Deleted<T>(T entity, string message = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message == null ? SharedResourcesKeys.Deleted : message
            };
        }
        public Response<T> Success<T>(T entity, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = SharedResourcesKeys.Success,
                Meta = Meta
            };
        }
        public Response<T> Unauthorized<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message == null ? SharedResourcesKeys.UnAuthorized : message
            };
        }
        public Response<T> BadRequest<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message == null ? "Bad Request" : message
            };
        }
        public Response<T> UnProcessableEntity<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message == null ? "UnpProcessableEntity" : message,
            };
        }

        public Response<T> NotFound<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? "Not Found" : message
            };
        }

        public Response<T> Created<T>(T entity, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = SharedResourcesKeys.Created,
                Meta = Meta
            };
        }

        public Response<T> Conflict<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Succeeded = false,
                Message = message ?? "Conflict"
            };
        }

        public Response<T> NoContent<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent,
                Succeeded = true,
                Message = message ?? "NoContent"
            };
        }
    }

}
