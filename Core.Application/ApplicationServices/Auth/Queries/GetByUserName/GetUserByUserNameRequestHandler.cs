using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetByUserName;

public sealed class GetUserByUserNameRequestHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetUserByUserNameQueryRequest, GetUserByUserNameQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByUserNameQueryResponse> Handle(GetUserByUserNameQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.FindByUserName(request.UserName);

        var response = user.Adapt<GetUserByUserNameQueryResponse>();
        return response;    
    }
}
