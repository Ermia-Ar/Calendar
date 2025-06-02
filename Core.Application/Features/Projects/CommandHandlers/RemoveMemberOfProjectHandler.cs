using AutoMapper;
using Core.Application.Exceptions.Project;
using Core.Application.Exceptions.UseRequest;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Enum;
using FluentValidation;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class RemoveMemberOfProjectHandler 
        : IRequestHandler<RemoveMemberOfProjectCommand, string>
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public RemoveMemberOfProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<string> Handle(RemoveMemberOfProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var project = await _unitOfWork.Projects
                .GetProjectById(request.ProjectId, cancellationToken);

            if (project.OwnerId != userId)
            {
                throw new OnlyProjectCreatorAllowedException();
            }
            //
            var receiver = await _unitOfWork.Users.FindByUserName(request.UserName);

            var userRequests = (await _unitOfWork.Requests.GetRequests(request.ProjectId, null, cancellationToken))
                .Where(x => x.ReceiverId == receiver.Id
                && x.Status == RequestStatus.Accepted)
                .ToList();

            if (!userRequests.Any())
            {
                throw new NotFoundMemberException("No such member was found.");
            }


            _unitOfWork.Requests.DeleteRangeRequests(userRequests);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Deleted";
        }
    }
}
