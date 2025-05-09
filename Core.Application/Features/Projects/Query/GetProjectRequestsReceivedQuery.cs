using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GetProjectRequestsReceivedQuery : IRequest<Response<List<ProjectRequestResponse>>>
    {
    }

}
