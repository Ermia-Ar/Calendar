using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public sealed class RemoveMemberOfProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
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

        var userRequests = await _unitOfWork.Requests.FindMember(request.ProjectId, null
            , userId, cancellationToken);

        if (!userRequests.Any())
        {
            throw new NotFoundMemberException("No such member was found.");
        }


        _unitOfWork.Requests.RemoveRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
