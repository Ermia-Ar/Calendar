using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequests.Commnads
{
    public class CreateRequestCommand : IRequest<Response<string>>
    {
        public SendActivityRequest Request { get; set; }
    }
}
