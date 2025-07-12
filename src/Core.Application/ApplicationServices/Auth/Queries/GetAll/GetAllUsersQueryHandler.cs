using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;


public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
            : IRequestHandler<GetAllUsersQueryRequest, PaginationResult<List<GetAllUserQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginationResult<List<GetAllUserQueryResponse>>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        //var users = await _unitOfWork.Users.GetAll(request.Filtering, request.Ordering
        //    , request.Pagination, cancellationToken);

        //var userResponse = users.Responses.Adapt<List<GetAllUserQueryResponse>>();

        //return new PaginationResult<List<GetAllUserQueryResponse>>(userResponse, request.Pagination.PageNumber
        //    , request.Pagination.PageSize, users.Count);

        throw new NotImplementedException();
    }
}
