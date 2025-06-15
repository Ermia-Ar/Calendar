using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll;

public sealed class GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
            : IRequestHandler<GetAllProjectsQueryRequest, List<GetAllProjectQueryResponse>>
{
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<GetAllProjectQueryResponse>> Handle(GetAllProjectsQueryRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUserServices.GetUserId();

        var projects = await _unitOfWork.Requests.GetProjects
            (ownerId, request.Filtering.UserIsOwner, cancellationToken
            , request.Filtering.StartDate, request.Filtering.IsHistory);

        var response = projects.Adapt<List<GetAllProjectQueryResponse>>();
        return response;
    }
}
