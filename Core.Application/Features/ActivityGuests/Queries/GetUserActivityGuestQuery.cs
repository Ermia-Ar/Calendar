using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.ActivityGuests.Queries
{
    public class GetUserActivityGuestQuery : IRequest<Response<List<ActivityResponse>>>
    {
    }
}
