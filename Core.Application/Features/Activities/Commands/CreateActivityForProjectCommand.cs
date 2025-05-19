using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class CreateActivityForProjectCommand : IRequest<Response<string>>
    {
        public CreateActivityForProjectRequest CreateActivity {  get; set; }
    }
}
