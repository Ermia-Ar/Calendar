using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.ActivityGuests.Queries
{
    public class GetActivityGuestByActivityIdQuery : IRequest<Response<List<UserResponse>>>
    {
        public string ActivityId { get; set; }
    }
}
