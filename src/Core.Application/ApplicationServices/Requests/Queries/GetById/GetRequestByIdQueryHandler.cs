using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Queries.GetById
{
    public sealed class GetRequestByIdQueryHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<GetRequestByIdQueryRequest, GetRequestByIdQueryResponse>
    {
        public IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GetRequestByIdQueryResponse> Handle(GetRequestByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userRequest = await _unitOfWork.ActivityRequests.GetById(request.Id, cancellationToken);

            return userRequest.Adapt<GetRequestByIdQueryResponse>();
        }
    }
}
