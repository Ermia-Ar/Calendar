using Core.Domain;
using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetRequestById
{
    public sealed class GetRequestByIdQueryHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<GetRequestByIdQueryRequest, GetRequestByIdQueryResponse>
    {
        public IUnitOfWork _unitOfWork = unitOfWork;

        public Task<GetRequestByIdQueryResponse> Handle(GetRequestByIdQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
