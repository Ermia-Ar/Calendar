using Core.Application.DTOs.ActivityDTOs;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class CreateActivityForProjectCommand : IRequest<string>
    {
        public CreateActivityForProjectRequest CreateActivity { get; set; }
    }
}
