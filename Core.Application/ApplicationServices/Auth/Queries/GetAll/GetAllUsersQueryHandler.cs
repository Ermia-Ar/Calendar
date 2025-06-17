using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll;


public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
            : IRequestHandler<GetAllUsersQueryRequest, List<GetAllUserQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<GetAllUserQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAll(request.Filtering.Search
            , request.Filtering.Category, cancellationToken);

        var userResponse = users.Adapt<List<GetAllUserQueryResponse>>();

        return userResponse;
    }
}
