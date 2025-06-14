using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll;

public sealed class GetUserProjectsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
            : IRequestHandler<GetUserProjectsQueryRequest, List<GetUserProjectQueryResponse>>
{
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<GetUserProjectQueryResponse>> Handle(GetUserProjectsQueryRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUserServices.GetUserId();

        var projects = await _unitOfWork.Requests.GetProjects
            (ownerId, request.UserIsOwner, cancellationToken, request.StartDate, request.IsHistory);

        var response = projects.Adapt<List<GetUserProjectQueryResponse>>();
        return response;
    }
}
