using AutoMapper;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.ExitingProject;

public class ExitingProjectCommandHandler
    : IRequestHandler<ExitingProjectCommandRequest>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IMapper _mapper;

    public ExitingProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserServices = currentUserServices;
    }

    public async Task Handle(ExitingProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var requests = (await _unitOfWork.Requests.GetRequests(request.ProjectId, null,
             _currentUserServices.GetUserId(), Domain.Enum.RequestStatus.Accepted,null, cancellationToken))
            .Adapt<List<UserRequest>>();

        if (!requests.Any())
        {
            throw new NotFoundMemberException("You are not a member of this project.");
        }

        _unitOfWork.Requests.DeleteRangeRequests(requests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
