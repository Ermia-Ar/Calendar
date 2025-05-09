using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public class DeleteProjectCommand : IRequest<Response<string>>
    {
        public string ProjcetId { get; set; }  
    }
}
