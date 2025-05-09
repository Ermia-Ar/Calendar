using AutoMapper;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class RequestAddMemberToProjectHandler : ResponseHandler,
        IRequestHandler<RequestAddMemberToProjectCommand, Response<string>>
    {
        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public RequestAddMemberToProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(RequestAddMemberToProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if project for user or not
            var isFor = await _unitOfWork.Projects.IsProjectForUser(request.ProjectRequest.ProjcetId, userId, cancellationToken);
            if (!isFor)
            {
                return BadRequest<string>("Project is not found !!");
            }
            // check UserNames exist ?
            foreach (var Receiver in request.ProjectRequest.Receivers)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(Receiver);
                if (isExist)
                {
                    return NotFound<string>($"user name {Receiver} does not exist !");
                }
            }
            //sent request for each members
            var addRequest = _mapper.Map<UserRequest>(request.ProjectRequest);
            addRequest.Id = Guid.NewGuid().ToString();
            addRequest.Sender = _currentUserServices.GetUserName();
            addRequest.Status = Domain.Enum.RequestStatus.Pending;
            addRequest.RequestFor = Domain.Enum.RequestFor.Project;
            addRequest.InvitedAt = DateTime.Now;
            addRequest.ProjectId = request.ProjectRequest.ProjcetId;
            addRequest.IsExpire = false;
            //send for each Receivers
            var userRequests = new List<UserRequest>();
            foreach (var receiver in request.ProjectRequest.Receivers)
            {
                addRequest.Receiver = receiver;
                userRequests.Add(addRequest);
            }
            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
