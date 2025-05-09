using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GetUnAnsweredProjectRequestQuery : IRequest<Response<List<ProjectRequestResponse>>>
    {
    }

}
