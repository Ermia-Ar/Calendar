using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetAll;

public class GetUserRequestQueryHandler(IUnitOfWork unitOfWork)
            : IRequestHandler<GetUserRequestsQueryRequest, List<GetUserRequestQueryResponse>>
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<GetUserRequestQueryResponse>> Handle(GetUserRequestsQueryRequest request, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.Requests.GetUserRequests
            (request.ProjectId, request.ActivityId
            , request.ReceiverId, request.SenderId
            , request.RequestFor, request.Status
            , request.IsExpire, cancellationToken);

        var response = request.Adapt<List<GetUserRequestQueryResponse>>();
        return response;
    }
}
