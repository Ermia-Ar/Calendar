using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequest.Commnads
{
    public class CreateRequestCommand : IRequest<Response<string>>
    {
        public SendRequest Request { get; set; }
    }
}
