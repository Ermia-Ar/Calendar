using AutoMapper;
using Core.Application.Exceptions.Project;
using Core.Application.Exceptions.User;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public sealed class RequestAddMemberToProjectHandler
        : IRequestHandler<RequestAddMemberToProjectCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public RequestAddMemberToProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<string> Handle(RequestAddMemberToProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if project for user or not
            var project = await _unitOfWork.Projects.GetProjectById(request.ProjectRequest.ProjectId, cancellationToken);
            if (project.OwnerId != userId)
            {
                throw new OnlyProjectCreatorAllowedException();
            }
            // check UserNames exist ?
            //foreach (var Receiver in request.ProjectRequest.Receivers)
            //{
            //    var isExist = await _unitOfWork.Users.IsUserNameExist(Receiver);
            //    if (!isExist)
            //    {
            //        throw new NotFoundException($"user name {Receiver} does not exist !");
            //    }
            //    //var userRequest = await _unitOfWork.Requests.GetTableNoTracking()
            //    //    .FirstOrDefaultAsync(x => x.RequestFor == Domain.Enum.RequestFor.Project
            //    //    && x.ProjectId == request.ProjectRequest.ProjcetId
            //    //    && x.ReceiverId == Receiver);
            //    //if (userRequest != null)
            //    //{
            //    //    if (userRequest.Status == Domain.Enum.RequestStatus.Accepted)
            //    //    {
            //    //        throw new BadRequestException($"user {Receiver} is already member of this project");
            //    //    }
            //    //    else if (userRequest.Status == Domain.Enum.RequestStatus.Pending)
            //    //    {
            //    //        throw new BadRequestException($"you already send this request for this user");
            //    //    }
            //    //}
            //}

            //sent request for each members
            var addRequest = _mapper.Map<UserRequest>(request.ProjectRequest);
            addRequest.Id = Guid.NewGuid().ToString();
            addRequest.Sender.Id = _currentUserServices.GetUserId();
            addRequest.Status = Domain.Enum.RequestStatus.Pending;
            addRequest.RequestFor = Domain.Enum.RequestFor.Project;
            addRequest.InvitedAt = DateTime.Now;
            addRequest.ProjectId = request.ProjectRequest.ProjectId;
            addRequest.IsExpire = false;

            //send for each Receivers
            var userRequests = new List<UserRequest>();
            foreach (var memberName in request.ProjectRequest.Receivers)
            {
                var receiver = await _unitOfWork.Users.FindByUserName(memberName);
                if (receiver != null)
                {
                    throw new NotFoundUserNameException(memberName);
                }
                addRequest.Id = Guid.NewGuid().ToString();
                addRequest.ReceiverId = receiver.Id;
                userRequests.Add(addRequest);
            }
            await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Created";
        }
    }
}
