using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public sealed class GetActivityByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<GetActivityByIdQueryRequest, GetActivityByIdQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<GetActivityByIdQueryResponse> Handle(GetActivityByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities
            .GetById(request.Id, cancellationToken);

        return activity.Adapt<GetActivityByIdQueryResponse>();
    }
}
