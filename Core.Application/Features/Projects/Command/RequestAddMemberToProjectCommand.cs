using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public class RequestAddMemberToProjectCommand : IRequest<Response<string>>
    {
       public SendProjectRequest ProjectRequest { get; set; }   
    }
}
