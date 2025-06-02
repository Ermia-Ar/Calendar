using Core.Application.DTOs.ActivityDTOs;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class CreateSubActivityCommand : IRequest<string>
    {
        public CreateSubActivityRequest CreateActivity { get; set; }
    }
}
