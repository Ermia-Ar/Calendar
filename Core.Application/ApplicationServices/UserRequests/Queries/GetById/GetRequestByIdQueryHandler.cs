using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetById
{
    public sealed class GetRequestByIdQueryHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<GetRequestByIdQueryRequest, GetRequestByIdQueryResponse>
    {
        public IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GetRequestByIdQueryResponse> Handle(GetRequestByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userRequest = await _unitOfWork.Requests.GetById(request.Id, cancellationToken);

            return userRequest.Adapt<GetRequestByIdQueryResponse>();
        }
    }
}
