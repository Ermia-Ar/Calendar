using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.SubmitRequest;

public sealed class SubmitProjectRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<SubmitProjectRequestCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(SubmitProjectRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //check if project for user or not
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project.OwnerId != userId)
        {
            throw new OnlyProjectCreatorAllowedException();
        }

        var addRequest = _mapper.Map<UserRequest>(request);
        addRequest.Id = Guid.NewGuid().ToString();
        addRequest.Sender.Id = _currentUserServices.GetUserId();
        addRequest.Status = Domain.Enum.RequestStatus.Pending;
        addRequest.RequestFor = Domain.Enum.RequestFor.Project;
        addRequest.InvitedAt = DateTime.Now;
        addRequest.ProjectId = request.ProjectId;
        addRequest.IsExpire = false;

        //send for each Receivers
        var userRequests = new List<UserRequest>();
        foreach (var memberName in request.Receivers)
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

        _unitOfWork.Requests.AddRange(userRequests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
