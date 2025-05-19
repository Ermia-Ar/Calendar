using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class CreateSubActivityCommand : IRequest<Response<string>>
    {
        public CreateSubActivityRequest CreateActivity {  get; set; }
    }
}
