using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public sealed class GetActivityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        : IRequestHandler<GetActivityByIdQueryRequest, GetActivityByIdQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<GetActivityByIdQueryResponse> Handle(GetActivityByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities
            .GetActivityById(request.Id, cancellationToken);

        return activity.Adapt<GetActivityByIdQueryResponse>();
    }
}
