using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Interfaces;
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

        //send for each Receivers
        var userRequests = new List<UserRequest>();
        foreach (var memberId in request.MemberIds)
        {
            var receiver = await _unitOfWork.Users.FindById(memberId);
            if (receiver == null)
            {
                throw new NotFoundUserIdException(memberId);
            }

            var sendRequest = RequestFactory.CreateProjectRequest(
                request.ProjectId, userId, memberId, request.Message
                );

            userRequests.Add(sendRequest);
        }

        _unitOfWork.Requests.AddRange(userRequests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
