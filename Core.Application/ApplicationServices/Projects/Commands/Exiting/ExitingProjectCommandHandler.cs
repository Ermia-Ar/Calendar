using AutoMapper;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

public class ExitingProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<ExitingProjectCommandRequest>
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(ExitingProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var requests = (await _unitOfWork.Requests.GetAll(request.ProjectId, null,
             _currentUserServices.GetUserId(), Domain.Enum.RequestStatus.Accepted,null, cancellationToken))
            .Adapt<List<UserRequest>>();

        if (!requests.Any())
        {
            throw new NotFoundMemberException("You are not a member of this project.");
        }

        _unitOfWork.Requests.RemoveRange(requests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
