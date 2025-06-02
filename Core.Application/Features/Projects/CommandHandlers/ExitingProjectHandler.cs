using AutoMapper;
using Core.Application.Exceptions.UseRequest;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers;

public class ExitingProjectHandler 
    : IRequestHandler<ExitingProjectCommand, string>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IMapper _mapper;

    public ExitingProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserServices = currentUserServices;
    }

    public async Task<string> Handle(ExitingProjectCommand request, CancellationToken cancellationToken)
    {
        var requests = (await _unitOfWork.Requests.GetRequests(request.ProjectId, null, cancellationToken))
           .Where(x => x.Receiver.Id == _currentUserServices.GetUserId()
           && x.Status == Domain.Enum.RequestStatus.Accepted)
           .ToList();

        if (!requests.Any())
        {
            throw new NotFoundMemberException("You are not a member of this project.");
        }

        _unitOfWork.Requests.DeleteRangeRequests(requests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return "Deleted";

    }
}
