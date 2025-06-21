using AutoMapper;
using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

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
        var userId = _currentUserServices.GetUserId();

        var requests = await _unitOfWork.Requests.FindMember(request.ProjectId, null
             , userId, cancellationToken);

        if (!requests.Any())
        {
            throw new NotFoundMemberException("You are not a member of this project.");
        }

        _unitOfWork.Requests.RemoveRange(requests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
