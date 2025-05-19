using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Exceptions;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class RequestAddMemberToProjectHandler : ResponseHandler,
        IRequestHandler<RequestAddMemberToProjectCommand, Response<string>>
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

        public async Task<Response<string>> Handle(RequestAddMemberToProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if project for user or not
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectRequest.ProjcetId, cancellationToken);
            if (project.OwnerId != userId)
            {
                throw new BadRequestException("Only the owner of this project has access to this section.");
            }
            // check UserNames exist ?
            foreach (var Receiver in request.ProjectRequest.Receivers)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(Receiver);
                if (isExist)
                {
                    throw new NotFoundException($"user name {Receiver} does not exist !");
                }
                var userRequest = await _unitOfWork.Requests.GetTableNoTracking()
                    .FirstOrDefaultAsync(x => x.RequestFor == Domain.Enum.RequestFor.Project
                    && x.ProjectId == request.ProjectRequest.ProjcetId
                    && x.Receiver == Receiver);

                if (userRequest != null)
                {
                    if (userRequest.Status == Domain.Enum.RequestStatus.Accepted)
                    {
                        throw new BadRequestException($"user {Receiver} is already member of this project");
                    }
                    else if (userRequest.Status == Domain.Enum.RequestStatus.Pending)
                    {
                        throw new BadRequestException($"you already send this request for this user");
                    }
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
            return Created("Created");
        }
    }
}
