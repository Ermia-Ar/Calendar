using Core.Domain.Interfaces;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetAll;

public sealed class GetAllRequestQueryHandler(IUnitOfWork unitOfWork)
            : IRequestHandler<GetAllRequestsQueryRequest, List<GetAllRequestQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<GetAllRequestQueryResponse>> Handle(GetAllRequestsQueryRequest request, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.Requests.GetAllRequests
            (request.Filtering.ProjectId, request.Filtering.ActivityId
            , request.Filtering.ReceiverId, request.Filtering.SenderId
            , request.Filtering.RequestFor, request.Filtering.Status
            ,cancellationToken);

        var response = request.Adapt<List<GetAllRequestQueryResponse>>();
        return response;
    }
}
