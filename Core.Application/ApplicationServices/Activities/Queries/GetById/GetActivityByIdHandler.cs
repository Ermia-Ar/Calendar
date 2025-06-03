using AutoMapper;
using Core.Domain;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public sealed class GetActivityByIdQueryHandler
    : IRequestHandler<GetActivityByIdQueryRequest, GetByIdActivityQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserServices _currentUserServices;

    public GetActivityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserServices = currentUserServices;
    }

    public async Task<GetByIdActivityQueryResponse> Handle(GetActivityByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities.GetActivityById(request.Id, cancellationToken);

        return activity.Adapt<GetByIdActivityQueryResponse>();
    }
}
