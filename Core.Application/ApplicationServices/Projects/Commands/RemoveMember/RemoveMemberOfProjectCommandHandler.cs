using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public class RemoveMemberOfProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<RemoveMemberOfProjectCommandRequest>
{

    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(RemoveMemberOfProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project.OwnerId != userId)
        {
            throw new OnlyProjectCreatorAllowedException();
        }
        //
        var receiver = await _unitOfWork.Users.FindByUserName(request.UserName);

        var userRequests = (await _unitOfWork.Requests
            .GetAll(request.ProjectId, null
            , userId, RequestStatus.Accepted
            , null, cancellationToken))
            .Adapt<List<UserRequest>>();

        if (!userRequests.Any())
        {
            throw new NotFoundMemberException("No such member was found.");
        }


        _unitOfWork.Requests.RemoveRange(userRequests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
