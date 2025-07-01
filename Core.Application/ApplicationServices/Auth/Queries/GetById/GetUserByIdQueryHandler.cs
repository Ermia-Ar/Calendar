using AutoMapper;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetById;

public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.FindById(request.Id);
        var userResponse = user.Adapt<GetUserByIdQueryResponse>();
        return userResponse;
    }
}
